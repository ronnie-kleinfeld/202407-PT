using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TdiInstall
{

    public partial class frmTdiInstall : Form
    {
        string UPDFolder = @"z:\UPD\";
        string SitesFolder = @"\\172.17.17.2\c$\ComDisplay_Sites\";
        string PublishFolder = @"\\172.17.17.2\c$\ComDisplay_Sites_test_Publish\TdiHebM\";

        int GridCounter = 1;

        private void InitProgress(int MaxValue)
        {
            lblProgress.Visible = true;
            ProgressBar.Visible = true;
            ProgressBar.Value = 0;
            ProgressBar.Maximum = MaxValue;

            Application.DoEvents();
        }

        private void BarUpdate()
        {
            ProgressBar.Value++;
            lblProgress.Text = $"Step {ProgressBar.Value} of {ProgressBar.Maximum}";

            Application.DoEvents();
        }


        private void BarUpdateAsync()
        {
            ProgressBar.Invoke(new Action(() =>
            {
                ProgressBar.Value++;
                lblProgress.Text = $"Step {ProgressBar.Value} of {ProgressBar.Maximum}";

                Application.DoEvents();
            }));
        }


        private void DoneProgress()
        {
            lblProgress.Visible = false;
            ProgressBar.Visible = false;

            Application.DoEvents();
        }


        public frmTdiInstall()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnInstallTDI_ClickAsync(object sender, EventArgs e)
        {
            AddToGrid("Delete previous install files");
            int FilesCount = DeletPreviousFiles();
            AddToGrid($"Deleted {FilesCount} files");

            AddToGrid("Backup sites folder");
            string TargetFolder = await BackupSitesFolderAsync();
            AddToGrid($"Backup folder was created:{Environment.NewLine} {TargetFolder}");

            AddToGrid("Publish the project");
            PublishWebProject();
            AddToGrid("Project was published");

            AddToGrid("Updating the UPD folder");
            int FilesAmount = UpdatingUPDFolder();
            AddToGrid($"UPD folder was updated: {UPDFolder}");
            AddToGrid($"{FilesAmount} files were copies to the UPD folder");

            string VersionFile = UPDFolder + "Version.txt";
            if (File.Exists(VersionFile))
            {
                AddToGrid($"Version number: {File.ReadAllLines(VersionFile)[1].Replace("AppVersion=Ver ", "")}", true);
            }

            AddToGrid("Updating the sites folders");
            UpdatingSitesFolders();
            AddToGrid($"Sites folders were updated from: {UPDFolder}");

            AddToGrid("Done!");
            MessageBox.Show("Done!");


        }

        private void UpdatingSitesFolders()
        {
            List<string> TdiFolders = Directory.GetDirectories(SitesFolder, "Tdi*").ToList();
            List<string> UpdatedFiles = Directory.GetFiles(UPDFolder, "*", SearchOption.AllDirectories).Where(x => Path.GetFileName(x) != "Defprop.config").ToList();

            foreach (string TdiFolder in TdiFolders)
            {
                AddToGrid($"Updating the folder: {TdiFolder}");

                InitProgress(UpdatedFiles.Count);
                foreach (string UpdatedFile in UpdatedFiles)
                {
                    BarUpdate();

                    File.Copy(UpdatedFile, UpdatedFile.Replace(UPDFolder, TdiFolder + "\\"), true);
                }
                DoneProgress();
            }
        }

        private int UpdatingUPDFolder()
        {
            string FirstTDIFolder = Directory.GetDirectories(SitesFolder, "Tdi*").First();
            DateTime MinDateTime = Directory.GetFiles(FirstTDIFolder, "*.*", SearchOption.AllDirectories).Select(d => new FileInfo(d).LastWriteTime).OrderByDescending(x => x).First();
            AddToGrid($"Minimum Date is {MinDateTime}", true);

            List<string> RelevantFiles = Directory.GetFiles(PublishFolder, "*", SearchOption.AllDirectories).Where(d => new FileInfo(d).LastWriteTime > MinDateTime).ToList();

            InitProgress(RelevantFiles.Count);
            foreach (string RelevantFile in RelevantFiles)
            {
                BarUpdate();
                File.Copy(RelevantFile, RelevantFile.Replace(PublishFolder, UPDFolder), true);
            }
            DoneProgress();

            File.SetLastWriteTime(UPDFolder + "Global.asax", DateTime.Now);
            return RelevantFiles.Count;
        }

        private void PublishWebProject()
        {
            string SolutionPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string SolutionFullPath = Path.Combine(SolutionPath, "DspDev2.sln");
            RunCommand($@" ""c:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\msbuild.exe""", $@"""{SolutionFullPath}"" /P:DeployOnBuild=true /P:PublishProfile=FolderProfile");
        }

        private void RunCommand(string FileName, string Arguments = "")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = FileName;
            startInfo.Arguments = Arguments;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
        }

        private async Task<string> BackupSitesFolderAsync()
        {
            string RootFolder = Directory.GetDirectoryRoot(SitesFolder);

            List<string> FolderList = Directory.GetDirectories(RootFolder, "*ComDisplay_Sites - Copy (*").ToList();

            int MaxNumber = FolderList.Select(x => int.Parse(Path.GetFileName(x).Replace("ComDisplay_Sites - Copy (", "").Replace(")", ""))).Max();

            string SourceFolder = SitesFolder;
            string TargetFolder = $"{RootFolder}\\ComDisplay_Sites - Copy ({MaxNumber + 1})\\";

            Directory.CreateDirectory(TargetFolder);

            List<string> SourceFoldersList = Directory.GetDirectories(SourceFolder, "*", SearchOption.AllDirectories).ToList();

            InitProgress(SourceFoldersList.Count);

            //foreach (string dirPath in SourceFoldersList)
            //{
            //    BarUpdate();
            //    Directory.CreateDirectory(dirPath.Replace(SourceFolder, TargetFolder));
            //}

            await Task.Run(() => Parallel.ForEach(SourceFoldersList, dirPath =>
                {
                    BarUpdateAsync();
                    Directory.CreateDirectory(dirPath.Replace(SourceFolder, TargetFolder));
                }));

            DoneProgress();

            //Copy all the files & Replaces any files with the same name
            List<string> FilesList = Directory.GetFiles(SourceFolder, "*.*", SearchOption.AllDirectories).ToList();
            InitProgress(FilesList.Count);


            //foreach (string SourcePath in FilesList)
            //{
            //    BarUpdate();
            //    File.Copy(SourcePath, SourcePath.Replace(SourceFolder, TargetFolder), true);
            //}

            await Task.Run(() => Parallel.ForEach(FilesList, SourcePath =>
            {
                BarUpdateAsync();
                File.Copy(SourcePath, SourcePath.Replace(SourceFolder, TargetFolder), true);
            }));

            DoneProgress();

            return TargetFolder;
        }

        private int DeletPreviousFiles()
        {
            int FilesCount = 0;
            List<string> FilesList = Directory.EnumerateFiles(UPDFolder, "*.*", SearchOption.AllDirectories).ToList();
            InitProgress(FilesList.Count);
            foreach (string fileName in FilesList)
            {
                BarUpdate();
                if (Path.GetFileName(fileName) != "Global.asax")
                {
                    File.Delete(fileName);
                    FilesCount++;
                }
            }
            DoneProgress();
            return FilesCount;
        }

        private void AddToGrid(string Message, bool pBold = false)
        {
            MyDgv.Rows.Add(GridCounter, DateTime.Now.ToString(), Message);

            if (pBold)
            {
                MyDgv.Rows[GridCounter - 1].DefaultCellStyle.Font = new Font(MyDgv.Font, FontStyle.Bold);
            }

            GridCounter++;
            this.Refresh();
            Application.DoEvents();
        }

        private void frmTdiInstall_Load(object sender, EventArgs e)
        {

        }
    }
}
