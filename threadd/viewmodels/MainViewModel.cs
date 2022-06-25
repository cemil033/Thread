using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace threadd.viewmodels
{
    public class MainViewModel:ViewModelBase
    {
        public MainViewModel() {  }

        public Thread thread;

        private string fromtxt;
        public string FromTxt
        {
            get => fromtxt; set
            {
                fromtxt = value;
                RaisePropertyChanged();
            }
        }

        private string totxt;
        public string ToTxt
        {
            get => totxt; set
            {
                totxt = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand FromBtnCommand
        {
            get => new RelayCommand
            (() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    FromTxt = openFileDialog.FileName;
            });
        }

        public RelayCommand ToBtnCommand
        {
            get => new RelayCommand
            (() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    ToTxt = openFileDialog.FileName;
            });
        }

        private int fSize;
        public int FSize
        {
            get => fSize; set
            {
                fSize = value;
                RaisePropertyChanged();
            }
        }

        private int curSize;
        public int CurSize
        {
            get => curSize; set
            {
                curSize = value;
                RaisePropertyChanged();
            }
        }
        public void Copy()
        {
            if (!File.Exists(FromTxt))
            {
                MessageBox.Show("File Not exists");
                return;
            }

            using (FileStream fsRead = new FileStream(FromTxt, FileMode.Open, FileAccess.Read))
            {
                Console.WriteLine($"Size {fsRead.Length} bytes");
                using (FileStream fsWrite = new FileStream(ToTxt, FileMode.Create, FileAccess.Write))
                {
                    var len = 10;
                    var fileSize = fsRead.Length;
                    FSize = (int)fileSize;
                    byte[] buffer = new byte[len];

                    do
                    {
                        len = fsRead.Read(buffer, 0, buffer.Length);
                        fsWrite.Write(buffer, 0, len);

                        fileSize -= len;
                        CurSize = FSize - (int)fileSize;

                        Thread.Sleep(500);

                    } while (len != 0);

                }
            }
            //FSize = 1000;
            //for (int i = 0; i < FSize; i++)
            //{
            //    CurSize = i;
            //    Thread.Sleep(100);
            //}
        }

        public RelayCommand CopyCommand
        {
            get => new RelayCommand(
                () =>
                {
                    try
                    {
                        thread = new Thread(Copy);
                        thread.Start();
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                );
        }
        [Obsolete]
        public RelayCommand SuspendCommand
        {
            get => new RelayCommand(
            () =>
            {

                try
                {
                    thread.Suspend();                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            );
        }

        [Obsolete]
        public RelayCommand ResumeCommand
        {
            get => new RelayCommand(
            () =>
            {
                try
                {
                    thread.Resume();                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            );
        }
        [Obsolete]
        public RelayCommand AbortCommand
        {
            get => new RelayCommand(
            () =>
            {

                try
                {
                    FSize = 0;
                    CurSize = 0;
                    thread.Abort();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            );
        }
    }
}
