using Blogging.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Blogging1.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer dispatcherTimer;

        public int[] getRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int tmp = 0;
            int sum = 0;
            for (int i = 0; i <= num - 2; i++)
            {
                tmp = ra.Next(minValue, maxValue); //随机取数
                arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra); //取出值赋到数组中
                sum += tmp;
            }
            arrNum[2] = 3 - sum;
            return arrNum;
        }

        public int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int id = Array.IndexOf(arrNum, tmp);
            if (id != -1)
            {
                tmp = ra.Next(minValue, maxValue);
                getNum(arrNum, tmp, minValue, maxValue, ra);
            }
        
            return tmp;
        }

        public MainPage()
        {
            this.InitializeComponent();
            //Storage default
            using (var db = new BloggingContext())
            {
                if (db.Blogs.ToList().Count < 3)
                {
                    storageDefaultFile();
                }


                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = TimeSpan.FromSeconds(10);
                dispatcherTimer.Start();
            }
        }

        public void dispatcherTimer_Tick(object sender, object e) {
            using (var db = new BloggingContext())
            {
                int[] arr = getRandomNum(3,0,3);
                List<Blog> newBlogs = db.Blogs.ToList();
                for (int i = 0; i < 3; i++)
                {
                    Blog blog = newBlogs[arr[i]];
                    string name = blog.Url;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelWidth = 100;
                    bitmapImage.UriSource = new Uri(name);
                    if (i == 0)
                    {
                        firstImage.Source = bitmapImage;
                    }
                    else if (i == 0)
                    {
                        secondImage.Source = bitmapImage;
                    }
                    else
                    {
                        thirdImage.Source = bitmapImage;
                    }

                }
            }
        }

        public void storageDefaultFile() { 
            using (var db = new BloggingContext())
                {
                for (int i = 0; i < 3; i++)
                {
                    string name = "Assets/" + (i+1) + ".jpg";
                    var blog = new Blog { Url = name };
                    db.Blogs.Add(blog);
                    db.SaveChanges();
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelWidth = 100;
                    bitmapImage.UriSource = new Uri(name);
                        if (i == 0)
                        {
                            firstImage.Source = bitmapImage;
                        }
                        else if (i == 0)
                        {
                            secondImage.Source = bitmapImage;
                        }
                        else
                        {
                            thirdImage.Source = bitmapImage;
                        }

                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new BloggingContext())
            {
                //Blogs.ItemsSource = db.Blogs.ToList();
            }
        }

        //private void Add_Click(object sender, RoutedEventArgs e)
        //{
        //        using (var db = new BloggingContext())
        //        {
        //            var blog = new Blog { Url = NewBlogUrl.Text };
        //db.Blogs.Add(blog);
        //            db.SaveChanges();

        //            Blogs.ItemsSource = db.Blogs.ToList();
        //        }
        //}
    }
}
