using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetWallpaperAn
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Run();
        }
        private void Run()
        {
            bool isEnglish = false;
            string originalImagePath = "C:\\Unikey\\wallpaper.jpg"; // Thay đổi đường dẫn ảnh gốc
            string newImagePath = "C:\\Unikey\\wallpaperdone.jpg"; // Đường dẫn lưu ảnh mới

            // Lấy thông tin máy
            string machineName = Environment.MachineName;
            string ipAddress = GetLocalIPAddress();
            string macAddress = GetMacAddress();
            string subnetMask = GetSubnetMask();
            string gateway = GetGateway();

            // Thông tin bản quyền
            string copyRight = "Thầy Thiện - ThienCNTT.com";

            // Tạo ảnh mới với thông tin
            using (Image img = Image.FromFile(originalImagePath))
            using (Bitmap bitmap = new Bitmap(img))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Font font = new Font("Tahoma", 14, FontStyle.Bold);
                Font font1 = new Font("Tahoma", 10, FontStyle.Bold);
                Brush brush1 = new SolidBrush(Color.Gold);
                Brush brush2 = new SolidBrush(Color.GreenYellow); Brush brush3 = new SolidBrush(Color.Silver);
                Brush brush4 = new SolidBrush(Color.Navy);
                Brush brush5 = new SolidBrush(Color.Indigo);
                Brush brush6 = new SolidBrush(Color.Blue);
                int dolechy = 30;

                // Vị trí đặt text
                // int x = bitmap.Width - 300; // điều chỉnh nếu cần
                int x = 24; // điều chỉnh nếu cần
                int y = bitmap.Height - 300; // điều chỉnh nếu cần

                // Điền thông tin vào ảnh

                if (isEnglish)
                {
                    g.DrawString($"Machine Name: {machineName}", font, brush1, x, y);
                    g.DrawString($"IP Address: {ipAddress}", font, brush2, x, y + dolechy);
                    g.DrawString($"MAC Address: {macAddress}", font, brush3, x, y + dolechy * 2);
                    g.DrawString($"Subnet Mask: {subnetMask}", font, brush4, x, y + dolechy * 3);
                    g.DrawString($"Gateway: {gateway}", font, brush5, x, y + dolechy * 4);
                    g.DrawString($"Code by {copyRight}", font1, brush6, bitmap.Width - 300, y + dolechy * 5);
                }
                else
                {
                    g.DrawString($"Tên máy tính: {machineName}", font, brush1, x, y);
                    g.DrawString($"Địa chỉ IP: {ipAddress}", font, brush2, x, y + dolechy);
                    g.DrawString($"Địa chỉ MAC: {macAddress}", font, brush3, x, y + dolechy * 2);
                    g.DrawString($"Mặt nạ mạng: {subnetMask}", font, brush4, x, y + dolechy * 3);
                    g.DrawString($"Bộ định tuyến: {gateway}", font, brush5, x, y + dolechy * 4);
                    g.DrawString($"Lập trình bởi {copyRight}", font1, brush6, bitmap.Width - 300, y + dolechy * 5);
                }

                // Lưu ảnh mới
                bitmap.Save(newImagePath, ImageFormat.Jpeg);
            }

            // Thiết lập làm hình nền
            SetWallpaper(newImagePath);

            // Đóng ứng dụng
            //Application.Exit();
            System.Environment.Exit(1);
        }

        static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Not available";
        }

        static string GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    return string.Join(":", nic.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
                }
            }
            return "Not available";
        }

        static string GetSubnetMask()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ip.IPv4Mask.ToString();
                        }
                    }
                }
            }
            return "Not available";
        }

        static string GetGateway()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    var gateway = nic.GetIPProperties().GatewayAddresses;
                    if (gateway.Count > 0)
                    {
                        return gateway[0].Address.ToString();
                    }
                }
            }
            return "Not available";
        }

        static void SetWallpaper(string path)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue("Wallpaper", path);
            key.Close();
            SystemParametersInfo(20, 0, path, 3);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
 }
