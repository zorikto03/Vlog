using NetCore3MVC.Enums;

namespace NetCore3MVC.Services
{
    public class AlertMaker
    {
        public static string ShowAlert(AlertType type, string message)
        {
            string alertDiv = type switch
            {
                AlertType.Success => "<div " +
                "class='alert alert-success alert-dismissable' " +
                "id='alert'>" +
                    "<button type='button' class='close' data-dismiss='alert'>x</button>" +
                    "<strong> Success! </strong>" + 
                    message +
                    "</a>." +
                "</div>",
                AlertType.Error => "<div " +
                "class='alert alert-danger alert-dismissable' " +
                "id='alert'>" +
                    "<button type='button' class='close' data-dismiss='alert'>x</button>" +
                    "<strong> Error! </strong>" +
                    message +
                    "</a>." +
                "</div>",
                AlertType.Warning => "<div " +
                "class='alert alert-warning alert-dismissable' " +
                "id='alert'>" +
                    "<button type='button' class='close' data-dismiss='alert'>x</button>" +
                    "<strong> Warning! </strong>" +
                    message +
                    "</a>." +
                "</div>",
                AlertType.Info => "<div " +
                "class='alert alert-info alert-dismissable' " +
                "id='alert'>" +
                    "<button type='button' class='close' data-dismiss='alert'>x</button>" +
                    "<strong> Info! </strong>" +
                    message +
                    "</a>." +
                "</div>",
                _ => string.Empty
            };
            return alertDiv;
        }
    }
}
