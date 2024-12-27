using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SCADACreator.Utility;
using SCADACreator.View;
using SCADACreator.Model;
using InTheHand.Net.Sockets;
using Microsoft.Win32;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Data.SqlTypes;
using SCADACreator.View.Alarm;
using System.Diagnostics;
using System.Data.SQLite;
using Path = System.IO.Path;
namespace SCADACreator
{
    public partial class MainWindow : Window
    {
        public PropertyPage propertyPage;
        public AnimationListPage animationListPage;
        public ItemEventListPage itemEventListPage;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            SCADADataProvider.Instance.ProjectInformation = new ProjectInformation();
            Title = $"SCADA Creator - {SCADADataProvider.Instance.ProjectInformation.Name}";
            LoadSCADAServerPATH();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (propertyPage == null) propertyPage = new PropertyPage();
            PropertyFrame.Content = propertyPage;
            if (animationListPage == null) animationListPage = new AnimationListPage();
            AnimationFrame.Content = animationListPage;
            if (itemEventListPage == null) itemEventListPage = new ItemEventListPage();
            ItemEventFrame.Content = itemEventListPage;
        }
        #region Design View
        public void SetCurrentItemForPage(DesignerItem designerItem)
        {
            propertyPage.SetCurrentItem(designerItem);
            animationListPage.SetCurrentItem(designerItem);
            itemEventListPage.SetCurrentItem(designerItem);
        }
        void setItemsize(DesignerItem item, FrameworkElement content)
        {
            if (content.MinHeight != 0 && content.MinWidth != 0)
            {
                item.Width = content.MinWidth * 2; ;
                item.Height = content.MinHeight * 2;
            }
            else
            {
                item.Width = 65;
                item.Height = 65;
            }
        }
        #endregion

        private string SerializeAllDesignerItems()
        {
            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            XElement serializedItems = new XElement("SCADAProject",
                 new XElement("TagInfos", JsonSerializer.Serialize(SCADADataProvider.Instance.TagInfos, options)),
                 new XElement("ConnectDevices", JsonSerializer.Serialize(SCADADataProvider.Instance.ConnectDevices, options)),
                 new XElement("AlarmSettings", JsonSerializer.Serialize(SCADADataProvider.Instance.AlarmSettings, options)),
                 new XElement("TagLoggingSettings", JsonSerializer.Serialize(SCADADataProvider.Instance.TagLoggingSettings, options)),
                 new XElement("TrendViewSettings", JsonSerializer.Serialize(SCADADataProvider.Instance.TrendViewSettings, options)),
                 new XElement("ProjectInformation", JsonSerializer.Serialize(SCADADataProvider.Instance.ProjectInformation, options)),
                 from page in SCADADataProvider.Instance.DesignPages
                 select new XElement("Pages",
                      from item in page.SCADAItems
                      let contentXaml = XamlWriter.Save(((SCADAItem)item).Content)
                      let tagconnectionjson = JsonSerializer.Serialize(((SCADAItem)item).TagConnection, options)
                      let animationsensejson = JsonSerializer.Serialize(((SCADAItem)item).AnimationSenses, options)
                      let itemeventjson = JsonSerializer.Serialize(((SCADAItem)item).ItemEvents, options)
                      select new XElement("SCADAItem",
                                  new XElement("Left", Canvas.GetLeft(item)),
                                  new XElement("Top", Canvas.GetTop(item)),
                                  new XElement("Width", item.Width),
                                  new XElement("Height", item.Height),
                                  new XElement("zIndex", Canvas.GetZIndex(item)),
                                  new XElement("Content", contentXaml),
                                  new XElement("AnimationSenses", animationsensejson),
                                  new XElement("ItemEvents", itemeventjson),
                                  new XElement("TagConnection", tagconnectionjson)
                                  ),
                      new XElement("Id", page.Id),
                      new XElement("Name", page.Name)
                       )
                  );
            return serializedItems.ToString();
        }
        private void DeserrializeSCADAData(string jsonString)
        {
            XElement parsedElement = XElement.Parse(jsonString);
            SCADADataProvider.Instance.ProjectInformation = JsonSerializer.Deserialize<ProjectInformation>((string)parsedElement.Element("ProjectInformation"));
            List<TagInfo> taginfos = JsonSerializer.Deserialize<List<TagInfo>>((string)parsedElement.Element("TagInfos"));
            List<ConnectDevice> connectedDevices = JsonSerializer.Deserialize<List<ConnectDevice>>((string)parsedElement.Element("ConnectDevices"));
            List<AlarmSetting> alarmSettings = JsonSerializer.Deserialize<List<AlarmSetting>>((string)parsedElement.Element("AlarmSettings"));
            List<TagLoggingSetting> tagLoggingSettings = JsonSerializer.Deserialize<List<TagLoggingSetting>>((string)parsedElement.Element("TagLoggingSettings"));
            List<TrendViewSetting> trendViewSettings = JsonSerializer.Deserialize<List<TrendViewSetting>>((string)parsedElement.Element("TrendViewSettings"));
            if (taginfos.Count > 0)
            {
                SCADADataProvider.Instance.AddListTagInfos(taginfos);
            }
            if (connectedDevices.Count > 0)
            {
                SCADADataProvider.Instance.AddListConnectDevices(connectedDevices);
            }
            if (alarmSettings.Count > 0)
            {
                SCADADataProvider.Instance.AddListAlarmSettings(alarmSettings);
            }
            if (tagLoggingSettings.Count > 0)
            {
                SCADADataProvider.Instance.AddListTagLoggingSettings(tagLoggingSettings);
            }
            if (trendViewSettings.Count > 0)
            {
                SCADADataProvider.Instance.AddListTrendViewSettings(trendViewSettings);
            }
        }
        private void DeserrializePagesData(string openstring)
        {
            List<DesignPage> designPages = new List<DesignPage>();
            XElement parsedElement = XElement.Parse(openstring);
            foreach (var pageElement in parsedElement.Elements("Pages"))
            {
                DesignPage page = new DesignPage();
                page.Id = (int)pageElement.Element("Id");
                page.Name = (string)pageElement.Element("Name");
                foreach (var itemElement in pageElement.Elements("SCADAItem"))
                {
                    SCADAItem scadaItem = new SCADAItem();
                    FrameworkElement content = XamlReader.Parse((string)itemElement.Element("Content")) as FrameworkElement;
                    scadaItem.Content = content;
                    scadaItem.Width = (double)itemElement.Element("Width");
                    scadaItem.Height = (double)itemElement.Element("Height");
                    scadaItem.TagConnection = JsonSerializer.Deserialize<TagInfo>((string)itemElement.Element("TagConnection"));
                    scadaItem.AnimationSenses = JsonSerializer.Deserialize<List<AnimationSense>>((string)itemElement.Element("AnimationSenses"));
                    scadaItem.ItemEvents = JsonSerializer.Deserialize<List<ItemEvent>>((string)itemElement.Element("ItemEvents"));
                    Canvas.SetLeft(scadaItem, (double)itemElement.Element("Left"));
                    Canvas.SetTop(scadaItem, (double)itemElement.Element("Top"));
                    Canvas.SetZIndex(scadaItem, (int)itemElement.Element("zIndex"));
                    page.SCADAItems.Add(scadaItem);
                }
                if (page.Id == SCADADataProvider.Instance.ProjectInformation.MainPageId)
                {
                    LoadDesignPage(page);
                }
                designPages.Add(page);
            }
            if (designPages.Count > 0)
            {
                SCADADataProvider.Instance.AddListDesignPages(designPages);
            }
        }
        private void ClearCurrentProject()
        {
            MyDesignerCanvas.Children.Clear();
            SCADADataProvider.Instance.Clear();
            SCADADataProvider.Instance.ProjectInformation = new ProjectInformation();
        }
        private void SaveProject()
        {
            //Save current Page
            currentPage.SCADAItems.Clear();
            foreach (SCADAItem item in MyDesignerCanvas.Children)
            {
                currentPage.SCADAItems.Add(item);
            }
            var savestring = SerializeAllDesignerItems();
            if (SCADADataProvider.Instance.ProjectInformation.IsNewProject)
            {
                SCADADataProvider.Instance.ProjectInformation.IsNewProject = false;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "untitled";
                saveFileDialog.Filter = "JsonString (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    Title = SCADADataProvider.Instance.ProjectInformation.Name = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                    string directoryPath = $"{System.IO.Path.GetDirectoryName(saveFileDialog.FileName)}\\{SCADADataProvider.Instance.ProjectInformation.Name}";
                    SCADADataProvider.Instance.ProjectInformation.FilePath = $"{directoryPath}\\{SCADADataProvider.Instance.ProjectInformation.Name}.json";
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    var sourceDbPath = ".\\..\\..\\DBOrigin\\scadastationorigin.db"; // Đường dẫn tới cơ sở dữ liệu gốc
                    var newDbPath = $"{directoryPath}\\{SCADADataProvider.Instance.ProjectInformation.Name}.db";
                    File.Copy(sourceDbPath, newDbPath);
                }
            }
            File.WriteAllText(SCADADataProvider.Instance.ProjectInformation.FilePath, savestring);//Save Json string to file
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                SaveProject();
            }
        }
        #region MenuEvent
        private void MenuItemNewSCADA_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentProject();
            Title = $"SCADA Creator - {SCADADataProvider.Instance.ProjectInformation.Name}";
        }
        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (SCADADataProvider.Instance.ProjectInformation.IsNewProject == true)
            {
                SaveProject();
            }
            else
            {
                var savestring = SerializeAllDesignerItems();
                SCADADataProvider.Instance.ProjectInformation.IsNewProject = false;
                var sourceDbPath = $"{System.IO.Path.GetDirectoryName(SCADADataProvider.Instance.ProjectInformation.FilePath)}\\{SCADADataProvider.Instance.ProjectInformation.Name}.db"; // Đường dẫn tới cơ sở dữ liệu gốc
                if (File.Exists(sourceDbPath))
                {
                    sourceDbPath = ".\\..\\..\\DBOrigin\\scadastationorigin.db";
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                //saveFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(SCADADataProvider.Instance.ProjectInformation.FilePath));
                saveFileDialog.FileName = "untitled";
                saveFileDialog.Filter = "JsonString (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    Title = SCADADataProvider.Instance.ProjectInformation.Name = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                    string directoryPath = $"{System.IO.Path.GetDirectoryName(saveFileDialog.FileName)}\\{SCADADataProvider.Instance.ProjectInformation.Name}";
                    SCADADataProvider.Instance.ProjectInformation.FilePath = $"{directoryPath}\\{SCADADataProvider.Instance.ProjectInformation.Name}.json";
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    //var sourceDbPath = ".\\..\\..\\DBOrigin\\scadastationorigin.db"; // Đường dẫn tới cơ sở dữ liệu gốc
                    var newDbPath = $"{directoryPath}\\{SCADADataProvider.Instance.ProjectInformation.Name}.db";
                    File.Copy(sourceDbPath, newDbPath);
                    File.WriteAllText(SCADADataProvider.Instance.ProjectInformation.FilePath, savestring);//Save Json string to file
                }
            }
        }
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveProject();
        }
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string jsonString = File.ReadAllText(openFileDialog.FileName);
                ClearCurrentProject();
                try
                {
                    DeserrializeSCADAData(jsonString);
                    DeserrializePagesData(jsonString);
                    SCADADataProvider.Instance.ProjectInformation.FilePath = openFileDialog.FileName;
                    SCADADataProvider.Instance.ProjectInformation.Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    this.Title = SCADADataProvider.Instance.ProjectInformation.Name;
                }
                catch
                {
                    MessageBox.Show("Cannot Open Project File");
                }
            }
        }
        private void MenuItemTags_Click(object sender, RoutedEventArgs e)
        {
            TagListWindow Tagwindow = new TagListWindow();
            Tagwindow.Show();
        }
        private void MenuItemDevices_Click(object sender, RoutedEventArgs e)
        {
            DeviceListWindow devicewindow = new DeviceListWindow();
            devicewindow.Show();
        }
        private void MenuItemStartSCADA_Click(object sender, RoutedEventArgs e)
        {
            StartProcess();
        }
        private void MenuItemAlarmSetting_Click(object sender, RoutedEventArgs e)
        {
            AlarmSettingWindow alarmListWindow = new AlarmSettingWindow();
            alarmListWindow.Show();
        }
        private void MenuItemTagLoggingSetting_Click(object sender, RoutedEventArgs e)
        {
            TagLoggingSettingWindow trendSettingWindow = new TagLoggingSettingWindow();
            trendSettingWindow.Show();
        }
        private void MenuItemTrendViewSetting_Click(object sender, RoutedEventArgs e)
        {
            TrendViewSettingWindow trendViewSettingWindow = new TrendViewSettingWindow();
            trendViewSettingWindow.Show();
        }
        private void MenuItemTablePageSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemControlsPageSetting_Click(object sender, RoutedEventArgs e)
        {
            PagesSettingWindow pagesSettingWindow = new PagesSettingWindow();
            pagesSettingWindow.OpenScreen += PagesSettingWindow_OpenScreen;
            pagesSettingWindow.Show();
        }

        private void PagesSettingWindow_OpenScreen(object sender, IntEventArgs e)
        {
            var page = SCADADataProvider.Instance.DesignPages.First(x => x.Id == e.Value);
            ChangePage(page);
        }

        private string SCADAServerPath;
        private void MenuItemSCADAServerPathSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingSCADAServerWindow settingSCADAPath = new SettingSCADAServerWindow(SCADAServerPath);
            settingSCADAPath.ApplyEvent += SettingSCADAPath_ApplyEvent;
            settingSCADAPath.ShowDialog();
        }

        private void SettingSCADAPath_ApplyEvent(object sender, StringEventArgs e)
        {
            SCADAServerPath = e.ServerPath;
            SaveSCADAServerPATH();
        }
        #endregion
        #region ToolBar Event
        private void OnClickBringToFront(object sender, RoutedEventArgs e)
        {
            foreach (DesignerItem item in MyDesignerCanvas.SelectedItems)
            {
                int maxZ = int.MinValue;
                foreach (DesignerItem child in MyDesignerCanvas.Children)
                {
                    if (child != item)
                    {
                        maxZ = Math.Max(maxZ, Canvas.GetZIndex(child));
                    }
                }
                Canvas.SetZIndex(item, maxZ + 1);
            }
        }
        private void OnClickSendToBack(object sender, RoutedEventArgs e)
        {
            foreach (DesignerItem item in MyDesignerCanvas.SelectedItems)
            {
                int maxZ = int.MaxValue;
                foreach (DesignerItem child in MyDesignerCanvas.Children)
                {
                    if (child != item)
                    {
                        maxZ = Math.Min(maxZ, Canvas.GetZIndex(child));
                    }
                }
                Canvas.SetZIndex(item, maxZ - 1);
            }
        }
        private void OnClickRotateLeft(object sender, RoutedEventArgs args)
        {
            Rotate(-90);
        }
        private void OnClickRotateRight(object sender, RoutedEventArgs args)
        {
            Rotate(90);
        }
        private void Rotate(double angle)
        {
            foreach (DesignerItem item in MyDesignerCanvas.SelectedItems)
            {
                FrameworkElement element = item.Content as FrameworkElement;
                if (element != null)
                {
                    RotateTransform rotateTransform = element.LayoutTransform as RotateTransform;
                    if (rotateTransform == null)
                    {
                        rotateTransform = new RotateTransform();
                        element.LayoutTransform = rotateTransform;
                    }
                    rotateTransform.Angle = (rotateTransform.Angle + angle) % 360;
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - (item.Height - item.Width) / 2);
                    Canvas.SetTop(item, Canvas.GetTop(item) - (item.Width - item.Height) / 2);
                    double width = item.Width;
                    item.Width = item.Height;
                    item.Height = width;
                }
            }
        }
        #endregion
        #region Start SCADA
        private void OnClickStartSCADA(object sender, RoutedEventArgs e)
        {
            StartProcess();
        }
        private void StartProcess()
        {
            if (SCADADataProvider.Instance.ProjectInformation.IsNewProject)
            {
                MessageBox.Show("Please save project before start");
            }
            else
            {
                var SCADAPages = ConvertDesignPageToSCADAPage(SCADADataProvider.Instance.DesignPages);
                string filename = $"{System.IO.Path.GetDirectoryName(SCADADataProvider.Instance.ProjectInformation.FilePath)}\\{SCADADataProvider.Instance.ProjectInformation.Name}Station.json";
                CreateSCADAStationFile(SCADAPages, filename);
                if (!File.Exists(SCADADataProvider.Instance.ProjectInformation.GetDBPath()))
                {
                    var sourceDbPath = ".\\DBOrigin\\scadastationorigin.db"; // Đường dẫn tới cơ sở dữ liệu gốc
                    var newDbPath = SCADADataProvider.Instance.ProjectInformation.GetDBPath();
                    File.Copy(sourceDbPath, newDbPath);
                }
                StartSCADAStation(filename);
            }
        }
        private void StartSCADAStation(string filename)
        {
            // Use ProcessStartInfo class
            Process proc = new Process();
            proc.StartInfo.FileName = SCADAServerPath; //Need update
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.Arguments = $"-f {filename}";
            proc.Start();
        }
        private void CreateSCADAStationFile(List<SCADAPage> scadaPages, string filename)
        {
            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            SCADAStationConfiguration mSCADAStationConfiguration = new SCADAStationConfiguration();
            mSCADAStationConfiguration.SetConnectDevices(SCADADataProvider.Instance.ConnectDevices);
            mSCADAStationConfiguration.SetTagInfos(SCADADataProvider.Instance.TagInfos);
            mSCADAStationConfiguration.SetAlarmSettings(SCADADataProvider.Instance.AlarmSettings);
            mSCADAStationConfiguration.SetTagLoggingSettings(SCADADataProvider.Instance.TagLoggingSettings);
            mSCADAStationConfiguration.SetTrendViewSettings(SCADADataProvider.Instance.TrendViewSettings);
            mSCADAStationConfiguration.ProjectInformation = (SCADADataProvider.Instance.ProjectInformation);
            mSCADAStationConfiguration.SetSCADAPages(scadaPages);
            string jsonSCADAStationConfiguration = JsonSerializer.Serialize(mSCADAStationConfiguration, options);//seriallize thành chuỗi json
            SaveFile(filename, jsonSCADAStationConfiguration);
        }
        private void SaveFile(string filePath, string savestring)
        {
            File.WriteAllText(filePath, savestring);//Save Json string to file
        }
        private List<ControlData> GenerateControlDataFromCanvas(DesignerCanvas designerCanvas)
        {
            UIElement[] canvasControl = designerCanvas.Children.Cast<UIElement>().ToArray();
            var controlDatas = new List<ControlData>();
            foreach (SCADAItem item in canvasControl)
            {
                ControlData controldata = ControlDataEncoder.Convert(item);
                controlDatas.Add(controldata);
            }
            controlDatas = controlDatas.OrderBy(m => m.ZIndex).ToList();
            return controlDatas;
        }
        private List<ControlData> GenerateControlDataFromSCADAPages(DesignPage page)
        {
            //UIElement[] canvasControl = designerCanvas.Children.Cast<UIElement>().ToArray();
            var controlDatas = new List<ControlData>();
            foreach (SCADAItem item in page.SCADAItems)
            {
                ControlData controldata = ControlDataEncoder.Convert(item);
                controlDatas.Add(controldata);
            }
            controlDatas = controlDatas.OrderBy(m => m.ZIndex).ToList();
            return controlDatas;
        }

        private List<SCADAPage> ConvertDesignPageToSCADAPage(List<DesignPage> designPages)
        {
            List<SCADAPage> scadaPages = new List<SCADAPage>();
            foreach (DesignPage page in SCADADataProvider.Instance.DesignPages)
            {
                var scadaPage = new SCADAPage();
                var controldatas = GenerateControlDataFromSCADAPages(page);
                scadaPage.Id = page.Id;
                scadaPage.Name = page.Name;
                //scadaPage.PageType = page.PageType;
                scadaPage.ControlDatas = controldatas;
                scadaPages.Add(scadaPage);
            }
            return scadaPages;
        }
        #endregion
        #region MultiPage
        private DesignPage currentPage;
        void LoadDesignPage(DesignPage page)
        {
            var SCADAItems = page.SCADAItems;
            MyDesignerCanvas.Children.Clear();
            foreach (var item in SCADAItems)
            {
                MyDesignerCanvas.Children.Add(item);
            }
            currentPage = page;
        }
        void ChangePage(DesignPage loadpage)
        {
            currentPage.SCADAItems.Clear();
            foreach (SCADAItem item in MyDesignerCanvas.Children)
            {
                currentPage.SCADAItems.Add(item);
            }
            LoadDesignPage(loadpage);
        }

        void LoadSCADAServerPATH()
        {
            string filePath = "DataProvider\\SCADAServerPATH.txt";
            SCADAServerPath = File.ReadAllText(filePath);
        }

        void SaveSCADAServerPATH()
        {
            string filePath = "DataProvider\\SCADAServerPATH.txt";
            File.WriteAllText(filePath, SCADAServerPath);

        }
        #endregion
        #region TestZone
        private void TestButton(object sender, RoutedEventArgs e)
        {
            var page = SCADADataProvider.Instance.DesignPages.First(x => x.Id != currentPage.Id);
            ChangePage(page);
        }
        private void TestButton2(object sender, RoutedEventArgs e)
        {

        }
        private void AddControButton_Click(object sender, RoutedEventArgs e)
        {
            SCADAItem Item1 = new SCADAItem();
            Image imageMotor1 = new Image();
            imageMotor1.MinWidth = 50;
            imageMotor1.MinHeight = 50;
            imageMotor1.Source = new BitmapImage(new Uri("../Images/pump_side_green.gif", UriKind.Relative));
            imageMotor1.IsHitTestVisible = false;
            Item1.Content = imageMotor1;
            setItemsize(Item1, imageMotor1);
            AnimationSense animation1 = new AnimationSense();
            animation1.Name = "testanmiationList1";
            animation1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.MemoryAddress == "DB1.DBX2.4").SingleOrDefault();
            animation1.Tagvaluemax = 0;
            animation1.Tagvaluemin = 0;
            animation1.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation1.PropertyBoolValueWhenTagInRange = false;
            AnimationSense animation1_2 = new AnimationSense();
            animation1_2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.MemoryAddress == "DB1.DBX2.4").SingleOrDefault();
            animation1_2.Name = "testanmiationList1_2";
            animation1_2.Tagvaluemax = 1;
            animation1_2.Tagvaluemin = 1;
            animation1_2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation1_2.PropertyBoolValueWhenTagInRange = true;
            Item1.AnimationSenses.Add(animation1);
            Item1.AnimationSenses.Add(animation1_2);
            Canvas.SetLeft(Item1, 50);
            Canvas.SetTop(Item1, 50);
            SCADAItem Item2 = new SCADAItem();
            Image imageMotor2 = new Image();
            imageMotor2.MinWidth = 50;
            imageMotor2.MinHeight = 50;
            imageMotor2.Source = new BitmapImage(new Uri("../Images/pump_side_grey.gif", UriKind.Relative));
            imageMotor2.IsHitTestVisible = false;
            Item2.Content = imageMotor2;
            setItemsize(Item2, imageMotor2);
            AnimationSense animation2 = new AnimationSense();
            animation2.Name = "testanmiationList2";
            animation2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.MemoryAddress == "DB1.DBX2.4").SingleOrDefault();
            animation2.Tagvaluemax = 0;
            animation2.Tagvaluemin = 0;
            animation2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation2.PropertyBoolValueWhenTagInRange = true;
            AnimationSense animation2_2 = new AnimationSense();
            animation2_2.Name = "testanmiationList2_2";
            animation2_2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.MemoryAddress == "DB1.DBX2.4").SingleOrDefault();
            animation2_2.Tagvaluemax = 1;
            animation2_2.Tagvaluemin = 1;
            animation2_2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation2_2.PropertyBoolValueWhenTagInRange = false;
            Item2.AnimationSenses.Add(animation2);
            Item2.AnimationSenses.Add(animation2_2);
            Canvas.SetLeft(Item2, 50);
            Canvas.SetTop(Item2, 50);
            SCADAItem Item3 = new SCADAItem();
            Image imageMotor3 = new Image();
            imageMotor3.MinWidth = 50;
            imageMotor3.MinHeight = 50;
            imageMotor3.Source = new BitmapImage(new Uri("../Images/pump_side_green.gif", UriKind.Relative));
            imageMotor3.IsHitTestVisible = false;
            Item3.Content = imageMotor3;
            setItemsize(Item3, imageMotor3);
            AnimationSense animation3 = new AnimationSense();
            animation3.Name = "testanmiationList3";
            animation3.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Status").SingleOrDefault();
            animation3.Tagvaluemax = 0;
            animation3.Tagvaluemin = 0;
            animation3.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation3.PropertyBoolValueWhenTagInRange = false;
            AnimationSense animation3_2 = new AnimationSense();
            animation3_2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Status").SingleOrDefault();
            animation3_2.Name = "testanmiationList3_2";
            animation3_2.Tagvaluemax = 1;
            animation3_2.Tagvaluemin = 1;
            animation3_2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation3_2.PropertyBoolValueWhenTagInRange = true;
            Item3.AnimationSenses.Add(animation3);
            Item3.AnimationSenses.Add(animation3_2);
            Canvas.SetLeft(Item3, 200);
            Canvas.SetTop(Item3, 50);
            SCADAItem Item4 = new SCADAItem();
            Image imageMotor4 = new Image();
            imageMotor4.MinWidth = 50;
            imageMotor4.MinHeight = 50;
            imageMotor4.Source = new BitmapImage(new Uri("../Images/pump_side_grey.gif", UriKind.Relative));
            imageMotor4.IsHitTestVisible = false;
            Item4.Content = imageMotor4;
            setItemsize(Item4, imageMotor4);
            AnimationSense animation4 = new AnimationSense();
            animation4.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Status").SingleOrDefault();
            animation4.Name = "testanmiationList4";
            animation4.Tagvaluemax = 0;
            animation4.Tagvaluemin = 0;
            animation4.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation4.PropertyBoolValueWhenTagInRange = true;
            AnimationSense animation4_2 = new AnimationSense();
            animation4_2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Status").SingleOrDefault();
            animation4_2.Name = "testanmiationList4_2";
            animation4_2.Tagvaluemax = 1;
            animation4_2.Tagvaluemin = 1;
            animation4_2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation4_2.PropertyBoolValueWhenTagInRange = false;
            Item4.AnimationSenses.Add(animation4);
            Item4.AnimationSenses.Add(animation4_2);
            Canvas.SetLeft(Item4, 200);
            Canvas.SetTop(Item4, 50);
            SCADAItem Item5 = new SCADAItem();
            TextBox label1 = new TextBox();
            label1.MinWidth = 50;
            label1.MinHeight = 50;
            label1.Text = "Text";
            label1.IsHitTestVisible = false;
            Item5.Width = 50;
            Item5.Height = 60;
            Item5.Content = label1;
            Item5.TagConnection = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Level_value").SingleOrDefault();
            //setItemsize(Item4, imageMotor4);
            Canvas.SetLeft(Item5, 150);
            Canvas.SetTop(Item5, 180);
            SCADAItem Item6 = new SCADAItem();
            Button button1 = new Button();
            button1.MinWidth = 50;
            button1.MinHeight = 50;
            var button1content = new TextBlock();
            button1content.Text = "start";
            button1.Content = button1content;
            button1.IsHitTestVisible = false;
            var btn1Content = new TextBlock();
            btn1Content.Text = "Start";
            button1.Content = btn1Content;
            Item6.Content = button1;
            setItemsize(Item6, button1);
            ItemEvent item1event1 = new ItemEvent();
            item1event1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_1_Start").SingleOrDefault();
            item1event1.Name = "Event 1";
            item1event1.ActionType = ItemEvent.ItemActiontype.emSetbit;
            item1event1.EventType = ItemEvent.ItemEventType.emPress;
            ItemEvent item1event2 = new ItemEvent();
            item1event2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_1_Start").SingleOrDefault();
            item1event2.Name = "Event 2";
            item1event2.ActionType = ItemEvent.ItemActiontype.emResetBit;
            item1event2.EventType = ItemEvent.ItemEventType.emRelease;
            Item6.ItemEvents.Add(item1event1);
            Item6.ItemEvents.Add(item1event2);
            Canvas.SetLeft(Item6, 50);
            Canvas.SetTop(Item6, 250);
            SCADAItem Item7 = new SCADAItem();
            Button button2 = new Button();
            button2.MinWidth = 50;
            button2.MinHeight = 50;
            button2.IsHitTestVisible = false;
            var btn2Content = new TextBlock();
            btn2Content.Text = "Stop";
            button2.Content = btn2Content;
            Item7.Content = button2;
            setItemsize(Item7, button2);
            ItemEvent item2event1 = new ItemEvent();
            item2event1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_1_Stop").SingleOrDefault();
            item2event1.Name = "Event 1";
            item2event1.ActionType = ItemEvent.ItemActiontype.emSetbit;
            item2event1.EventType = ItemEvent.ItemEventType.emPress;
            ItemEvent item2event2 = new ItemEvent();
            item2event2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_1_Stop").SingleOrDefault();
            item2event2.Name = "Event 2";
            item2event2.ActionType = ItemEvent.ItemActiontype.emResetBit;
            item2event2.EventType = ItemEvent.ItemEventType.emRelease;
            Item7.ItemEvents.Add(item2event1);
            Item7.ItemEvents.Add(item2event2);
            Canvas.SetLeft(Item7, 50);
            Canvas.SetTop(Item7, 350);
            SCADAItem Item8 = new SCADAItem();
            Button button3 = new Button();
            button3.MinWidth = 50;
            button3.MinHeight = 50;
            button3.IsHitTestVisible = false;
            var btn3Content = new TextBlock();
            btn3Content.Text = "Start";
            button3.Content = btn3Content;
            Item8.Content = button3;
            setItemsize(Item8, button3);
            ItemEvent item3event1 = new ItemEvent();
            item3event1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Start").SingleOrDefault();
            item3event1.Name = "Event 1";
            item3event1.ActionType = ItemEvent.ItemActiontype.emSetbit;
            item3event1.EventType = ItemEvent.ItemEventType.emPress;
            ItemEvent item3event2 = new ItemEvent();
            item3event2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Start").SingleOrDefault();
            item3event2.Name = "Event 2";
            item3event2.ActionType = ItemEvent.ItemActiontype.emResetBit;
            item3event2.EventType = ItemEvent.ItemEventType.emRelease;
            Item8.ItemEvents.Add(item3event1);
            Item8.ItemEvents.Add(item3event2);
            Canvas.SetLeft(Item8, 220);
            Canvas.SetTop(Item8, 250);
            SCADAItem Item9 = new SCADAItem();
            Button button4 = new Button();
            button4.MinWidth = 50;
            button4.MinHeight = 50;
            button4.IsHitTestVisible = false;
            var btn4Content = new TextBlock();
            btn4Content.Text = "Stop";
            button4.Content = btn4Content;
            Item9.Content = button4;
            setItemsize(Item9, button2);
            ItemEvent item4event1 = new ItemEvent();
            item4event1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Stop").SingleOrDefault();
            item4event1.Name = "Event 1";
            item4event1.ActionType = ItemEvent.ItemActiontype.emSetbit;
            item4event1.EventType = ItemEvent.ItemEventType.emPress;
            ItemEvent item4event2 = new ItemEvent();
            item4event2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Stop").SingleOrDefault();
            item4event2.Name = "Event 2";
            item4event2.ActionType = ItemEvent.ItemActiontype.emResetBit;
            item4event2.EventType = ItemEvent.ItemEventType.emRelease;
            Item9.ItemEvents.Add(item4event1);
            Item9.ItemEvents.Add(item4event2);
            Canvas.SetLeft(Item9, 220);
            Canvas.SetTop(Item9, 350);
            //SCADAItem Item8 = new SCADAItem();
            //Ellipse ellipse1 = new Ellipse();
            //ellipse1.MinWidth = 50;
            //ellipse1.MinHeight = 50;
            ////ellipse1.Source = new BitmapImage(new Uri("../Images/pump_side_green.gif", UriKind.Relative));
            //ellipse1.IsHitTestVisible = false;
            //ellipse1.Fill = System.Windows.Media.Brushes.Red;
            //Item8.Content = ellipse1;
            //setItemsize(Item8, ellipse1);
            //AnimationSense animatione1 = new AnimationSense();
            //animatione1.Tagname = "DB1.DBX2.4";
            //animatione1.Tagvaluemax = 0;
            //animatione1.Tagvaluemin = 0;
            //animatione1.PropertyNeedChange = AnimationSense.PropertyType.emBackgroundcColor;
            //animatione1.ColorWhenTagInRange = new ColorRGB(((SolidColorBrush)System.Windows.Media.Brushes.Red).Color);
            //AnimationSense animatione1_2 = new AnimationSense();
            //animatione1_2.Tagname = "DB1.DBX2.4";
            //animatione1_2.Tagvaluemax = 1;
            //animatione1_2.Tagvaluemin = 1;
            //animatione1_2.PropertyNeedChange = AnimationSense.PropertyType.emBackgroundcColor;
            //animatione1_2.ColorWhenTagInRange = new ColorRGB(((SolidColorBrush)System.Windows.Media.Brushes.Blue).Color);
            //Item8.animationSenses.Add(animatione1);
            //Item8.animationSenses.Add(animatione1_2);
            //Canvas.SetLeft(Item8, 350);
            //Canvas.SetTop(Item8, 350);
            //SCADAItem Item9 = new SCADAItem();
            //Line line1 = new Line();
            //line1.MinWidth = 50;
            //line1.MinHeight = 50;
            ////ellipse1.Source = new BitmapImage(new Uri("../Images/pump_side_green.gif", UriKind.Relative));
            //line1.IsHitTestVisible = false;
            //line1.Stroke = System.Windows.Media.Brushes.Red;
            //line1.X1 = 0;
            //line1.Y1 = 0;
            //line1.X2 = 50;
            //line1.Y2 = 50;
            //Item9.Content = line1;
            //setItemsize(Item9, line1);
            //Canvas.SetLeft(Item9, 350);
            //Canvas.SetTop(Item9, 50);
            //TextBox 
            //    entry   
            MyDesignerCanvas.Children.Add(Item2);
            MyDesignerCanvas.Children.Add(Item1);
            MyDesignerCanvas.Children.Add(Item4);
            MyDesignerCanvas.Children.Add(Item3);
            MyDesignerCanvas.Children.Add(Item5);
            MyDesignerCanvas.Children.Add(Item6);
            MyDesignerCanvas.Children.Add(Item7);
            MyDesignerCanvas.Children.Add(Item8);
            MyDesignerCanvas.Children.Add(Item9);
        }

        #endregion


    }
}
