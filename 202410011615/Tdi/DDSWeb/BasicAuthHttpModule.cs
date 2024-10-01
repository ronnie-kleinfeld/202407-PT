using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;



/// <summary>
/// Summary description for BasicAuthHttpModule
/// </summary>
public class BasicAuthHttpModule : IHttpModule
{
    private const string Realm = "localhost";
  
    public void Init(HttpApplication context)
    {
      
        // Register event handlers
        //context.AuthorizeRequest += Context_AuthorizeRequest;            
        context.AuthenticateRequest += Context_AuthenticateRequest;
        context.EndRequest += OnApplicationEndRequest;
    }

    string AuthenticationHeaderValue(string authHeader)
    {
        string usernamePassword = "";
        if (authHeader != null && authHeader.StartsWith("Basic"))
        {
            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

        }
        else
        {
            //Handle what happens if that isn't the case
            throw new Exception("The authorization header is either empty or isn't Basic.");
        }

        return usernamePassword;
    }

    private  void Context_AuthenticateRequest(object sender, EventArgs e)
    {
 
        var request = HttpContext.Current.Request;
        var response = HttpContext.Current.Response;
        var authHeader = request.Headers["Authorization"];
       
        if (authHeader != null)
        {
         
           var authHeaderVal = AuthenticationHeaderValue(authHeader);
          //  throw new NotImplementedException(authHeaderVal.Parameter);
            // RFC 2617 sec 1.2, "scheme" name is case-insensitive
            // if (authHeaderVal.Scheme.Equals("basic",
            //         StringComparison.OrdinalIgnoreCase) &&
            //     authHeaderVal.Parameter != null)
            // {
            AuthenticateUser(authHeaderVal);
           // }
        }
        else
        {
            //pop up a basic authentication window
            HttpContext.Current.Response.StatusCode = 401;
        }
       
    }




    private  void SetPrincipal(IPrincipal principal)
    {
        Thread.CurrentPrincipal = principal;
        if (HttpContext.Current != null)
        {
            HttpContext.Current.User = principal;
            
        }
    }

    // TODO: Here is where you would validate the username and password.
    private  bool CheckPassword(string username, string password)
    {
        return true;

        //username == "user" && password == "password";
    }

    private  void AuthenticateUser(string credentials)
    {
        try
        {
            //var encoding = Encoding.GetEncoding("iso-8859-1");
            //credentials = encoding.GetString(Convert.FromBase64String(credentials));

            int separator = credentials.IndexOf(':');
            string name = credentials.Substring(0, separator);
            string password = credentials.Substring(separator + 1);

            if (CheckPassword(name, password))
            {
                var identity = new GenericIdentity(name);
                SetPrincipal(new GenericPrincipal(identity, null));
                HttpContext.Current.Request.Headers.Add("pass", password);
            }
            else
            {
                // Invalid username or password.
                HttpContext.Current.Response.StatusCode = 401;
                
            }
        }
        catch (FormatException)
        {
            HttpContext.Current.Response.StatusCode = 401;
            //Credentials were not formatted correctly.
            
        }
    }

    // If the request was unauthorized, add the WWW-Authenticate header 
    // to the response.
    private  void OnApplicationEndRequest(object sender, EventArgs e)
    {
        var response = HttpContext.Current.Response;
        if (response.StatusCode == 401)
        {
            response.Headers.Add("WWW-Authenticate",
                string.Format("Basic realm=\"{0}\"", Realm));
        }
    }

    public void Dispose()
    {
    }
}