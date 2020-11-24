    using System; 
    using System.Runtime.InteropServices; 
     
    namespace Resolution
    { 
        [StructLayout(LayoutKind.Sequential)] 
        public struct DEVMODE1 
        { 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
            public string dmDeviceName; 
            public short dmSpecVersion; 
            public short dmDriverVersion; 
            public short dmSize; 
            public short dmDriverExtra; 
            public int dmFields; 
     
            public short dmOrientation; 
            public short dmPaperSize; 
            public short dmPaperLength; 
            public short dmPaperWidth; 
     
            public short dmScale; 
            public short dmCopies; 
            public short dmDefaultSource; 
            public short dmPrintQuality; 
            public short dmColor; 
            public short dmDuplex; 
            public short dmYResolution; 
            public short dmTTOption; 
            public short dmCollate; 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
            public string dmFormName; 
            public short dmLogPixels; 
            public short dmBitsPerPel; 
            public int dmPelsWidth; 
            public int dmPelsHeight; 
     
            public int dmDisplayFlags; 
            public int dmDisplayFrequency; 
     
            public int dmICMMethod; 
            public int dmICMIntent; 
            public int dmMediaType; 
            public int dmDitherType; 
            public int dmReserved1; 
            public int dmReserved2; 
     
            public int dmPanningWidth; 
            public int dmPanningHeight; 
        }; 
     
        class User_32 
        { 
            [DllImport("user32.dll")] 
            public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE1 devMode); 
            [DllImport("user32.dll")] 
            public static extern int ChangeDisplaySettings(ref DEVMODE1 devMode, int flags); 
     
            public const int ENUM_CURRENT_SETTINGS = -1; 
            public const int CDS_UPDATEREGISTRY = 0x01; 
            public const int CDS_TEST = 0x02; 
            public const int DISP_CHANGE_SUCCESSFUL = 0; 
            public const int DISP_CHANGE_RESTART = 1; 
            public const int DISP_CHANGE_FAILED = -1; 
            public const int DM_PELS_WIDTH = 0x80000;
            public const int DM_PELS_HEIGHT = 0x100000;
            public const int DM_DISPLAY_FREQUENCY = 0x400000;
        } 
     
        public class PrimaryScreenResolution 
        { 
            static public string ChangeResolution(int width, int height, int frequency) 
            { 
                DEVMODE1 dm = GetDevMode1(); 

                int iRet = User_32.EnumDisplaySettings(null, User_32.ENUM_CURRENT_SETTINGS, ref dm);
                if (iRet == 0)
                {
                    return string.Format("Failed To Pass The Test of The Resolution. Code: {0}", iRet); 
                }

                dm.dmPelsWidth = width; 
                dm.dmPelsHeight = height; 
                dm.dmDisplayFrequency = frequency;
                dm.dmFields = User_32.DM_PELS_WIDTH | User_32.DM_PELS_HEIGHT | User_32.DM_DISPLAY_FREQUENCY;

                iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_TEST);
                if (iRet == User_32.DISP_CHANGE_FAILED)
                {
                    return "Unable To Process Your Request. Sorry For This Inconvenience."; 
                }

                iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_UPDATEREGISTRY); 
                switch (iRet) 
                { 
                    case User_32.DISP_CHANGE_SUCCESSFUL: 
                        { 
                            return "Success"; 
                        } 
                    case User_32.DISP_CHANGE_RESTART: 
                        { 
                            return "You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode."; 
                        } 
                    default: 
                        { 
                            return string.Format("Failed To Change The Resolution. Code: {0}", iRet); 
                        } 
                } 
            } 
     
            private static DEVMODE1 GetDevMode1() 
            { 
                DEVMODE1 dm = new DEVMODE1(); 
                dm.dmDeviceName = new String(new char[32]); 
                dm.dmFormName = new String(new char[32]); 
                dm.dmSize = (short)Marshal.SizeOf(dm); 
                return dm; 
            } 
        } 
    } 
