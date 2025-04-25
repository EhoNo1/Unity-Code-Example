using UnityEngine;

/// <summary>
/// Enables persistence of player input device configurations between scenes.
/// </summary>
public static class PlayerConfigurationData
{
    /*const int maxLocalPlayers = 4;

    private static InputDevice[] playerInputDevices = new InputDevice[maxLocalPlayers];

    public static void SetPlayerInputDevice(int player, InputDevice device)
    {
        if (player < 0 && player > maxLocalPlayers - 1) 
        {
            Debug.LogWarning("The max numbers of local players is " + maxLocalPlayers + ", cannot set the input device of player number " + player);
            return;
        }

        playerInputDevices[player] = device;
    }


    public static InputDevice[] GetInputDevices()
    {
        return playerInputDevices;
    }*/
}
