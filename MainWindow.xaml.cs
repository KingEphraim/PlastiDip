using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCSharpSDK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            USB.IsChecked = true;
            resultcanvas.Visibility = Visibility.Hidden;
            Expandbutton.Visibility = Visibility.Hidden;
        }




        public void BasicandDeviceParameters(PaymentEngine.xTransaction.Request TransRequest)
        {
            try
            {
                TransRequest.xKey = Key.Text;
                TransRequest.xVersion = "4.5.9";
                TransRequest.xSoftwareName = "PlastiDip";
                TransRequest.xSoftwareVersion = "2.0";




                TransRequest.EnableDeviceInsertSwipeTap = (bool)EnableDeviceCKBox.IsChecked;
                TransRequest.EnableTipPrompt = (bool)EnableTipPromptCKBox.IsChecked;
                TransRequest.EnableSilentMode = (bool)EnableSilentModeCKBox.IsChecked;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

            if (USB.IsChecked == true)
            {
                TransRequest.Device_Set(DeviceName.Text, COMPort.Text, Baud.Text, "N", "8");
                
            }
            else
            {
                TransRequest.Device_Set(DeviceName.Text, IP.Text, IPPort.Text);
            }

        }
        public void BasicandDeviceParametersReport(xCore.xGateway.Report.Request TransRequest)
        {

            TransRequest.xKey = Key.Text;
            TransRequest.xVersion = "4.5.9";
            TransRequest.xSoftwareName = "PlastiDip";
            TransRequest.xSoftwareVersion = "2.0";






        }


        private void ShowItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();
                BasicandDeviceParameters(TransRequest);
                TransRequest.xTax = 1.26m;
                TransRequest.xAmount = 19.23m;

                string ItemsToSHow = "[{'xdescription':'ItemABC','xupc':558,'xqty':3,'xunit':'EA','xunitprice':'5.99'}]";
                TransRequest.UpdateItems("json", ItemsToSHow);

                TransRequest.Device_ShowItems();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void GetSignatureButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();
                BasicandDeviceParameters(TransRequest);
                string MySignature = TransRequest.GetSignature("Please sign");
                MessageBox.Show(MySignature, "Your Signature");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        public void AdditionalFields(PaymentEngine.xTransaction.Request TransRequest)
        {
            TransRequest.xName = Namee.Text;
            TransRequest.xCurrency = Currency.Text;
            TransRequest.xEmail = Email.Text;
            TransRequest.xInvoice = Invoice.Text;
            TransRequest.xStreet = Street.Text;
            TransRequest.xZip = Zip.Text;
            TransRequest.xDescription = Description.Text;

        }
        private void Sale_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "cc:sale";
            AdditionalFields(TransRequest);
            decimal.TryParse(Amount.Text, out decimal input);
            TransRequest.xAmount =input;
            PaymentEngine.xTransaction.Response TransResponse = TransRequest.ProcessOutOfScope();
            xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
            resulttextbox.Text = TransResponse.FormattedResponse();
            if (TransResponse.xResult == "A")
            {
                resulttextbox.Background = Brushes.Green;
            }
            else
            {
                resulttextbox.Background = Brushes.Red;
            }
            resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
            resultcanvas.Visibility = Visibility.Visible;


            RefNum.Text = TransResponse.xRefNum;
        }

        private void ShowWelcomeScreenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                TransRequest.Device_ShowWelcomeScreen();
            }
             catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void USB_Checked(object sender, RoutedEventArgs e)
        {
            COMPort.Visibility = Visibility.Visible ;
            Baud.Visibility = Visibility.Visible;
            IP.Visibility = Visibility.Hidden;
            IPPort.Visibility = Visibility.Hidden;
        }

        private void IP_Checked(object sender, RoutedEventArgs e)
        {
            COMPort.Visibility = Visibility.Hidden;
            Baud.Visibility = Visibility.Hidden;
            IP.Visibility = Visibility.Visible;
            IPPort.Visibility = Visibility.Visible;
        }

        private void Void_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "cc:void";
            try
            {
                
                long RefNumlong = Convert.ToInt64(RefNum.Text);
                TransRequest.xRefNum = RefNumlong;
                PaymentEngine.xTransaction.Response TransResponse = TransRequest.Process();
                xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
                resulttextbox.Text = TransResponse.FormattedResponse();
                if (TransResponse.xResult == "A")
                {
                    resulttextbox.Background = Brushes.Green;
                }
                else
                {
                    resulttextbox.Background = Brushes.Red;
                }
                resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
                resultcanvas.Visibility = Visibility.Visible;
            }
            catch (Exception error)
            {
                
                resulttextbox.Background = Brushes.Red;

                resulttextbox.Text = "Please verify refnum." + Environment.NewLine + "(" + error.Message + ")";
                resultcanvas.Visibility = Visibility.Visible;
                
            }
            
            
            

        }

        private void Refund_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "cc:refund";
            try
            {
                long RefNumlong = Convert.ToInt64(RefNum.Text);
                TransRequest.xRefNum = RefNumlong;
                decimal.TryParse(Amount.Text, out decimal input);
                TransRequest.xAmount = input;
                PaymentEngine.xTransaction.Response TransResponse = TransRequest.Process();
                xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
                resulttextbox.Text = TransResponse.FormattedResponse();
                if (TransResponse.xResult == "A")
                {
                    resulttextbox.Background = Brushes.Green;
                }
                else
                {
                    resulttextbox.Background = Brushes.Red;
                }
                resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
                resultcanvas.Visibility = Visibility.Visible;
            }
            catch (Exception error)
            {
                resulttextbox.Background = Brushes.Red;

                resulttextbox.Text = "Please verify refnum." + Environment.NewLine + "(" + error.Message + ")";
                resultcanvas.Visibility = Visibility.Visible;
            }
        }

        private void Credit_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "cc:credit";
            
            decimal.TryParse(Amount.Text, out decimal input);
            TransRequest.xAmount = input;
            PaymentEngine.xTransaction.Response TransResponse = TransRequest.ProcessOutOfScope();
            xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
            resulttextbox.Text = TransResponse.FormattedResponse();
            if (TransResponse.xResult == "A")
            {
                resulttextbox.Background = Brushes.Green;
            }
            else
            {
                resulttextbox.Background = Brushes.Red;
            }
            resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
            resultcanvas.Visibility = Visibility.Visible;
            RefNum.Text = TransResponse.xRefNum;
        }

        private void GetSignatureAndSaveToFile_Click(object sender, RoutedEventArgs e)
        {

            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();
            BasicandDeviceParameters(TransRequest);
            string MySignature = TransRequest.GetSignatureAndSaveToFile("Signature.png",true, "Please sign");
            MessageBox.Show(MySignature, "Your Signature");

            
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "cc:authonly";
           
            decimal.TryParse(Amount.Text, out decimal input);
            TransRequest.xAmount = input;
            PaymentEngine.xTransaction.Response TransResponse = TransRequest.ProcessOutOfScope();
            xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
            resulttextbox.Text = TransResponse.FormattedResponse();
            if (TransResponse.xResult == "A")
            {
                resulttextbox.Background = Brushes.Green;
            }
            else
            {
                resulttextbox.Background = Brushes.Red;
            }
            resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
            resultcanvas.Visibility = Visibility.Visible;
            RefNum.Text = TransResponse.xRefNum;
        }

        private void Capture_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "cc:capture";
            decimal.TryParse(Amount.Text, out decimal input);
            TransRequest.xAmount = input;
            try
            {
                long RefNumlong = Convert.ToInt64(RefNum.Text);
                TransRequest.xRefNum = RefNumlong;
                PaymentEngine.xTransaction.Response TransResponse = TransRequest.Process();
                xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
                resulttextbox.Text = TransResponse.FormattedResponse();
                if (TransResponse.xResult == "A")
                {
                    resulttextbox.Background = Brushes.Green;
                }
                else
                {
                    resulttextbox.Background = Brushes.Red;
                }
                resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
                resultcanvas.Visibility = Visibility.Visible;
            }
            catch (Exception error)
            {
                resulttextbox.Background = Brushes.Red;

                resulttextbox.Text = "Please verify refnum." + Environment.NewLine + "(" + error.Message + ")";
                resultcanvas.Visibility = Visibility.Visible;
            }
        }

        private void CheckOS_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();
            BasicandDeviceParameters(TransRequest);
            try
            {
                bool checkos = TransRequest.Device_OsUpdateAvailable();
                if (checkos)
                {
                    MessageBox.Show("OS update is available.");
                }
                else
                {
                    MessageBox.Show("OS is up to date.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckFirmware_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();
            BasicandDeviceParameters(TransRequest);
            try
            {
                bool checkfirmware = TransRequest.Device_FirmwareUpdateAvailable();
                if (checkfirmware)
                {
                    MessageBox.Show("Firmware update is available.");
                }
                else
                {
                    MessageBox.Show("Firmware is up to date.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateOS_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();
            BasicandDeviceParameters(TransRequest);
            try
            {
                TransRequest.Device_UpdateOS();
                MessageBox.Show("Please wait for the device to boot up", "Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void UpdateFirmware_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();


            BasicandDeviceParameters(TransRequest);
            try
            {
                TransRequest.Device_UpdateFirmware();
                MessageBox.Show("Please wait for the device to boot up", "Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            xCore.xGateway.Report.Request TransRequest = new xCore.xGateway.Report.Request();

            BasicandDeviceParametersReport(TransRequest);
            TransRequest.xCommand = reporttype.Text;
            TransRequest.xBeginDate = begindate.Text;

            TransRequest.xEndDate_Set(DateTime.Today.AddDays(1));
            xCore.xGateway.Report.Response TransResponse = TransRequest.Process();
            if (!string.IsNullOrEmpty(TransResponse.xError))
            {
                MessageBox.Show(TransResponse.FormattedResponse());
            }
            else if (TransResponse.xRecordsReturned == 0)
            {
                MessageBox.Show("No Records Found");
            }
            else
            {

                
                DataTable MyDV = TransResponse.xReportData_DataTable;
                reportgrid.DataContext = MyDV.DefaultView;
                
            }
        }

        private void Begindate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            _ = picker.SelectedDate;
        }

        private void Device_PromptEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_PromptEntry();
                MessageBox.Show(form, "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_PromptForConfirmation("Line one", "Line two");
                MessageBox.Show(form, "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form_Copy4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_PromptForEmail();
                MessageBox.Show(form, "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form_Copy7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_PromptForPhone_JSON();
                MessageBox.Show(form, "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_PromptForZip();
                MessageBox.Show(form, "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form_Copy3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                TransRequest.Device_ShowLongMessage("Cardknox is the leading, developer-friendly payment gateway integration provider for in-store, online, or mobile transactions hassle-free. Cardknox is the leading, developer-friendly payment gateway integration provider for in-store, online, or mobile transactions hassle-free. Integrating EMV payments has never been this simple & flexible. Tap into our native EMV certifications, bypass common challenges, and get ready to take advantage of fast and secure chip - card payments with our leading out -of - scope POS integration. No payment integration is too complex. Our solutions make even the most difficult integrations that much easier, so you can get exactly what you need in a lot less time. LET'S BE PARTNERS Join the industry's most aggressive and rewarding partner program with full marketing, onboarding and white - glove support, so you can add and retain more merchants.");
                MessageBox.Show("Done", "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form_Copy6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_ShowLongMessageWithConfirmation("Cardknox is the leading, developer-friendly payment gateway integration provider for in-store, online, or mobile transactions hassle-free. Cardknox is the leading, developer-friendly payment gateway integration provider for in-store, online, or mobile transactions hassle-free. Integrating EMV payments has never been this simple & flexible. Tap into our native EMV certifications, bypass common challenges, and get ready to take advantage of fast and secure chip - card payments with our leading out -of - scope POS integration. No payment integration is too complex.Our solutions make even the most difficult integrations that much easier, so you can get exactly what you need in a lot less time. LET'S BE PARTNERS Join the industry's most aggressive and rewarding partner program with full marketing, onboarding and white - glove support, so you can add and retain more merchants.", "Fine","Lol");
                MessageBox.Show(form, "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form_Copy5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                TransRequest.Device_ShowMessage("Hello","Welcome to","Cardknox","Thank you");
                MessageBox.Show("Done", "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form_Copy2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_GetScreenShot();
                MessageBox.Show(form, "formresult");
                System.IO.File.WriteAllText("screenshot.txt", form);

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void DeviceInitialize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_Initialize();
                MessageBox.Show(form,"formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void DeviceForceInitialize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

                BasicandDeviceParameters(TransRequest);
                string form = TransRequest.Device_ForceInitialize();
                MessageBox.Show(form, "formresult");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void EBTFSSALE_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "ebtfs:sale";
            AdditionalFields(TransRequest);
            decimal.TryParse(Amount.Text, out decimal input);
            TransRequest.xAmount = input;
            PaymentEngine.xTransaction.Response TransResponse = TransRequest.ProcessOutOfScope();
            xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
            resulttextbox.Text = TransResponse.FormattedResponse();
            if (TransResponse.xResult == "A")
            {
                resulttextbox.Background = Brushes.Green;
            }
            else
            {
                resulttextbox.Background = Brushes.Red;
            }
            resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
            resultcanvas.Visibility = Visibility.Visible;


            RefNum.Text = TransResponse.xRefNum;
        }

        private void EBTFSBALANCE_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "ebtfs:balance";
            AdditionalFields(TransRequest);        
            PaymentEngine.xTransaction.Response TransResponse = TransRequest.ProcessOutOfScope();
            xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
            resulttextbox.Text = TransResponse.FormattedResponse();
            if (TransResponse.xResult == "A")
            {
                resulttextbox.Background = Brushes.Green;
            }
            else
            {
                resulttextbox.Background = Brushes.Red;
            }
            resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
            resultcanvas.Visibility = Visibility.Visible;


            RefNum.Text = TransResponse.xRefNum;
        }

        private void Expandbutton_Click(object sender, RoutedEventArgs e)
        {
            resultcanvas.Visibility = Visibility.Visible;
        }

        private void Minimizebutton_Click(object sender, RoutedEventArgs e)
        {
            resultcanvas.Visibility = Visibility.Hidden;
            Expandbutton.Visibility = Visibility.Visible;
        }

        private void ebtfs_credit_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine.xTransaction.Request TransRequest = new PaymentEngine.xTransaction.Request();

            BasicandDeviceParameters(TransRequest);
            TransRequest.xCommand = "ebtfs:credit";
            AdditionalFields(TransRequest);
            decimal.TryParse(Amount.Text, out decimal input);
            TransRequest.xAmount = input;
            PaymentEngine.xTransaction.Response TransResponse = TransRequest.ProcessOutOfScope();
            xCore.ymwDictionary TransResponseDictionary = new xCore.ymwDictionary(TransResponse, xCore.ymwDictionary.EnumListTypes.Properties);
            resulttextbox.Text = TransResponse.FormattedResponse();
            if (TransResponse.xResult == "A")
            {
                resulttextbox.Background = Brushes.Green;
            }
            else
            {
                resulttextbox.Background = Brushes.Red;
            }
            resulttextboxfull.Text = TransResponseDictionary.ToCopy(true).ToSlack();
            resultcanvas.Visibility = Visibility.Visible;


            RefNum.Text = TransResponse.xRefNum;
        }
    }
}
