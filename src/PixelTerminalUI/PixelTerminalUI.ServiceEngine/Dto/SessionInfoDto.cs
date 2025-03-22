using PixelTerminalUI.ConsoleAdapter.Helpers;
using PixelTerminalUI.ServiceEngine.Models;

namespace PixelTerminalUI.ServiceEngine.Dto;

public class SessionInfoDto
{
    public string SessionUid { get; set; }
    public int FormHeight { get; set; }
    public int FormWidth { get; set; }
    public string? MenuCode { get; set; }
    public string DisplayedInfo { get; set; }
    public string? SavedDisplayedInfo { get; set; }
    public string UserLogin { get; set; }
    public string? UserInput { get; set; }
    public int UserInputWdith { get; set; }

    public SessionInfoDto()
    {
    }

    public SessionInfoDto(SessionInfo sessionInfo)
    {
        if (sessionInfo == null)
        {
            throw new ArgumentNullException(nameof(sessionInfo));
        }

        bool displayBorders = sessionInfo.DisplayBorders;
        
        SessionUid = sessionInfo.SessionUid;
        FormHeight = sessionInfo.FormHeight;
        FormWidth = sessionInfo.FormWidth;
        MenuCode = sessionInfo.MenuCode;
        DisplayedInfo = ConsoleHelper.GetDisplayedInfoString(sessionInfo.DisplayedInfo, displayBorders);
        if (sessionInfo.SavedDisplayedInfo != null)
        {
            SavedDisplayedInfo = ConsoleHelper.GetDisplayedInfoString(sessionInfo.SavedDisplayedInfo, displayBorders);
        }
        UserLogin = sessionInfo.UserLogin;
        UserInput = sessionInfo.UserInput;
        UserInputWdith = sessionInfo.CurrentForm?.FocusedEditControl?.Width ?? sessionInfo.FormWidth;
    }
}