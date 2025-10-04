# PixelTerminalUI

[English](README.md) | [Русский](README.ru.md)

## Background

PixelTerminalUI was inspired by my experience working as a C# developer at a large IT company that operated a major online store of digital equipment. I worked in the WMS department, developing applications for internal logistics. One of the key applications was a legacy Telnet UI application used for interacting with handheld terminals (PDAs).

As the company sought to modernize its development practices with CI/CD, challenges arose in building and deploying .NET Framework 4.8 applications. So we started exploring the possibilities of moving our major projects to newer .NET versions (eg .NET 6/8) and I decided to take the Telnet UI application into my area of responsibility.

Intrigued by how this application achieved a full UI experience using simple characters, I decided to explore its inner workings. This led to the creation of `PixelTerminalUI`, a project aimed at reimagining the engine behind this Telnet UI application. It serves as a demonstration of how to create a character-based UI framework in modern .NET.

The Telnet UI application established a persistent connection between the client and the server, which could complicate the issue of scalability and performance of the service. So, as `PixelTerminalUI` evolved, I also decided to try to use a REST API instead of Telnet/TCP and improve the session management mechanism.

`PixelTerminalUI` is a completely independent project, developed in my free time and does not contain any code or confidential information from my previous employer.

## Usage examples

Example of displaying information in a console application:

```
------------------------------------
                                    |
                                    |
                                    |
                                    |
             WELCOME TO             |
         PIXEL TERMINAL UI          |
                                    |
                                    |
                                    |
                                    |
                                    |
                                    |
                                    |
                                    |
....................................|
                                    |
                                    |
PRESS ENTER TO CONTINUE             |
------------------------------------
```

Displaying and processing of the form, as well as its visual elements/controls, is performed mainly on the server side; the client application often only displays the result received from the server. Also, when defining a form, it is possible to implement methods for processing user input (via the `EnterValidation` delegate, inherited from the base form `BaseForm`).

Example code for the form:

```C#
public class frmStart : BaseForm
{
    private TextControl? lblWelcome;
    private TextControl? lblAppName;

    private TextEditControl? txtUserInput;

    public frmStart() : base()
    {
    }
    
    protected override void InitializeComponent()
    {
        Name = nameof(frmStart);
        
        lblWelcome = new TextControl();
        lblWelcome.Name = nameof(lblWelcome);
        lblWelcome.Top = 4;
        lblWelcome.Left = 0;
        lblWelcome.EntireLine = true;
        lblWelcome.HorizontalAlignment = HorizontalAlignment.Center;
        lblWelcome.Value = "WELCOME TO";
        Controls.Add(lblWelcome);

        lblAppName = new TextControl();
        lblAppName.Name = nameof(lblAppName);
        lblAppName.Top = 5;
        lblAppName.Left = 0;
        lblAppName.EntireLine = true;
        lblAppName.HorizontalAlignment = HorizontalAlignment.Center;
        lblAppName.Value = "PIXEL TERMINAL UI";
        Controls.Add(lblAppName);

        txtUserInput = new TextEditControl();
        txtUserInput.Name = nameof(txtUserInput);
        txtUserInput.Top = 14;
        txtUserInput.Left = 0;
        txtUserInput.EntireLine = true;
        txtUserInput.Hint = "PRESS ENTER TO CONTINUE";
        txtUserInput.EnterValidation = txtUserInput_EnterValidation;
        Controls.Add(txtUserInput);
    }

    private bool txtUserInput_EnterValidation()
    {
        try
        {
            switch (txtUserInput.Value)
            {
                case "":
                case "-n":
                    ShowForm(new frmLogin());
                    break;

                case "-q":
                case "-b":
                    ShowInformation("Are you sure to exit the application?");
                    break;
            }
        }
        finally
        {
            txtUserInput.Value = "";
        }
        return true;
    }
}
```

## Architecture

Interaction between project modules:

![PixelTerminalUI.architecture](docs/img/PixelTerminalUI.architecture.png)

The requirements are presented at the link: [requirements](docs/requirements.ru.md)
