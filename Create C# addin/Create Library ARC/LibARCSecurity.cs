using System;
using System.IO;
using System.Text;
using Autodesk.Revit.DB;
using Microsoft.Win32;


namespace NS.LibARC
{
    public class LibARCSecurity
    {

        public static string GetProcessorName()
        {
            // Đường dẫn đến khóa Registry chứa thông tin bộ xử lý
            string registryPath = @"HARDWARE\DESCRIPTION\System\CentralProcessor\0";

            try
            {
                // Mở khóa Registry để đọc thông tin
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath))
                {
                    if (key != null)
                    {
                        // Lấy giá trị của "Identifier"
                        object identifierValue = key.GetValue("Identifier");

                        object vendorIdentifierValue = key.GetValue("VendorIdentifier");

                        // Kiểm tra và hiển thị giá trị
                        if (identifierValue != null)
                        {
                            string identifier = identifierValue.ToString();

                            string vendorIdentifier = vendorIdentifierValue.ToString();


                            return identifier + ", " + vendorIdentifier;
                            //Console.WriteLine(identifier);
                        }
                        else
                        {
                            //Console.WriteLine("Identifier value not found.");
                            return ("Identifier value not found.");
                        }
                    }
                    else
                    {
                        return ("Registry key not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return ("An error occurred: " + ex.Message);
            }
        }

        //Chuyển từ mã Hex sang Byte 
        static byte[] ConvertHexStringToByteArray(string hexString)
        {
            int numberOfChars = hexString.Length;
            byte[] bytes = new byte[numberOfChars / 2];
            for (int i = 0; i < numberOfChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static bool CheckLicense()
        {
            string processorName = GetProcessorName();

            // Đường dẫn đến file text
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // Kết hợp với các thư mục tiếp theo trong đường dẫn của bạn
            string filePath = Path.Combine(appDataPath, @"pyRevit\pyRevit_pyrevit.dll");
            try
            {
                // Đọc toàn bộ nội dung file
                string fileContent = File.ReadAllText(filePath);

                int startIndex = fileContent.IndexOf('\'') + 1;
                int endIndex = fileContent.LastIndexOf('\'');

                bool check = false;

                if (startIndex > 0 && endIndex > startIndex)
                {
                    // Lấy ký tự nằm giữa hai dấu '
                    string extractedContent = fileContent.Substring(startIndex, endIndex - startIndex);

                    //Bỏ 2 ký tự cuối
                    string modifiedString = extractedContent.Remove(extractedContent.Length - 2);

                    byte[] bytes = ConvertHexStringToByteArray(modifiedString);

                    // Chuyển đổi byte array thành chuỗi văn bản
                    string text = Encoding.ASCII.GetString(bytes);

                    //Khai báo biến check


                    if (text == processorName)
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                    }
                    return check;

                }
                else
                {
                    return check;
                }
            }
            catch
            {
                return false;
            }

        }

        //static void Main()
        //{

        //    bool kiemTra = CheckLicense();
        //    Console.WriteLine(kiemTra);

        //}
    }
}



