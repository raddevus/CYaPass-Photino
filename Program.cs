using PhotinoNET;
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace HelloPhotinoApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var version = "1.3.1";
            // Window title declared here for visibility
            string windowTitle = $"CYaPass : v{version} ";

            // Creating a new PhotinoWindow instance with the fluent API
            var window = new PhotinoWindow()
                .SetTitle(windowTitle)
                // Resize to a percentage of the main monitor work area
                .SetUseOsDefaultSize(false)
                .SetSize(new Size(820, 600))
                // Center window in the middle of the screen
                .Center()
                // Users can resize windows by default.
                // Let's make this one fixed instead.
                .SetResizable(false)
                .SetIconFile(@"wwwroot\cyapass.ico")
                .RegisterCustomSchemeHandler("app", (object sender, string scheme, string url, out string contentType) =>
                {
                    contentType = "text/javascript";
                    return new MemoryStream(Encoding.UTF8.GetBytes(@"
                        (() =>{
                            window.setTimeout(() => {
                                alert(`🎉 Dynamically inserted JavaScript.`);
                            }, 1000);
                        })();
                    "));
                })
                // Most event handlers can be registered after the
                // PhotinoWindow was instantiated by calling a registration 
                // method like the following RegisterWebMessageReceivedHandler.
                // This could be added in the PhotinoWindowOptions if preferred.
                .RegisterWebMessageReceivedHandler((object sender, string message) => {
                    var window = (PhotinoWindow)sender;

                    // The message argument is coming in from sendMessage.
                    // "window.external.sendMessage(message: string)"
                    string response = $"Received message: \"{message}\"";

                    // Send a message back the to JavaScript event handler.
                    // "window.external.receiveMessage(callback: Function)"
                    window.SendWebMessage(response);
                })
                .Load("wwwroot/index.htm"); // Can be used with relative path strings or "new URI()" instance to load a website.

            window.WaitForClose(); // Starts the application event loop
        }
    }
}
