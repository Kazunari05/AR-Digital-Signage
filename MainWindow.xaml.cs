using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using Geis.Win32;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Controls;

namespace KinectMouse
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kinect;
        Storyboard sb, sb2, sb3, sb4;
        Boolean game_state = false; //ゲームのフラグ
        readonly int Bgr32BytesPerPixel = PixelFormats.Bgr32.BitsPerPixel / 8;
        //const int R = 5;        // 手の位置を描画する円の大きさ

        //ゲーム関連
        int TopPosition = 0, LeftPosition = 200;
        int TopPosition2 = -36, LeftPosition2 = 900;
        int TopPosition3 = -36, LeftPosition3 = 350;
        int TopPosition4 = 10, LeftPosition4 = 450;
        int TopPosition5 = 5, LeftPosition5 = 650;
        int TopPosition6 = -40, LeftPosition6 = 500;
        int total_point = 0;
        int time = 0, timeUp = 0;
        int peach_flag = 0;

        //おしらせボタンのフラグ
        Boolean oshirase = false;

        //storyboard宣言
        Storyboard sbhomein, sbhomeout, sbhomead, sbhomegame,
          sbad1, sbadin, sbadout, sbad2, sbad3, sbad4,
          sbaddisin, sbaddisout, sbaddisb1, sbaddisb2, sbaddisbin, sbaddisbout;

        public MainWindow()
        {
            InitializeComponent();

            var adpicture1 = new BitmapImage();

            int homeflag = 0, adflag = 0, adflag2 = 0,adpic =1;

            sbhomein = new Storyboard();
            sbhomeout = new Storyboard();
            sbhomead = new Storyboard();
            sbhomegame = new Storyboard();
            sbad1 = new Storyboard();
            sbadin = new Storyboard();
            sbadout = new Storyboard();
            sbad2 = new Storyboard();
            sbad3 = new Storyboard();
            sbad4 = new Storyboard();
            sbaddisin = new Storyboard();
            sbaddisout = new Storyboard();
            sbaddisb1 = new Storyboard();
            sbaddisb2 = new Storyboard();
            sbaddisbin = new Storyboard();
            sbaddisbout = new Storyboard();

            DoubleAnimation had = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            had.RepeatBehavior = new RepeatBehavior(2);
            had.AutoReverse = true;
            DoubleAnimation hgame = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            hgame.RepeatBehavior = new RepeatBehavior(2);
            hgame.AutoReverse = true;
            DoubleAnimation homein = new DoubleAnimation(-400, 350, TimeSpan.FromSeconds(1));
            DoubleAnimation homeout = new DoubleAnimation(350, -400, TimeSpan.FromSeconds(1));

            DoubleAnimation ad1 = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            ad1.RepeatBehavior = new RepeatBehavior(2);
            ad1.AutoReverse = true;
            DoubleAnimation ad1in = new DoubleAnimation(-1000, -200, TimeSpan.FromSeconds(1));
            DoubleAnimation ad1in2 = new DoubleAnimation(-1000, -100, TimeSpan.FromSeconds(1));
            DoubleAnimation ad1out = new DoubleAnimation(-200, -1000, TimeSpan.FromSeconds(1));
            DoubleAnimation ad1out2 = new DoubleAnimation(-100, -1000, TimeSpan.FromSeconds(1));
            DoubleAnimation ad2 = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            ad2.RepeatBehavior = new RepeatBehavior(2);
            ad2.AutoReverse = true;
            DoubleAnimation ad2in = new DoubleAnimation(-1000, -200, TimeSpan.FromSeconds(1));
            DoubleAnimation ad2out = new DoubleAnimation(-200, -1000, TimeSpan.FromSeconds(1));
            DoubleAnimation ad2in2 = new DoubleAnimation(-1000, -100, TimeSpan.FromSeconds(1));
            DoubleAnimation ad2out2 = new DoubleAnimation(-100, -1000, TimeSpan.FromSeconds(1));
            DoubleAnimation ad3 = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            ad3.RepeatBehavior = new RepeatBehavior(2);
            ad3.AutoReverse = true;
            DoubleAnimation ad3in = new DoubleAnimation(-1000, -200, TimeSpan.FromSeconds(1));
            DoubleAnimation ad3out = new DoubleAnimation(-200, -1000, TimeSpan.FromSeconds(1));
            DoubleAnimation ad3in2 = new DoubleAnimation(-1000, -100, TimeSpan.FromSeconds(1));
            DoubleAnimation ad3out2 = new DoubleAnimation(-100, -1000, TimeSpan.FromSeconds(1));
            DoubleAnimation adhome = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            adhome.RepeatBehavior = new RepeatBehavior(2);
            adhome.AutoReverse = true;
            DoubleAnimation adhomein = new DoubleAnimation(-1000, -200, TimeSpan.FromSeconds(1));
            DoubleAnimation adhomeout = new DoubleAnimation(-200, -1000, TimeSpan.FromSeconds(1));
            DoubleAnimation adhomein2 = new DoubleAnimation(-2000, -100, TimeSpan.FromSeconds(1));
            DoubleAnimation adhomeout2 = new DoubleAnimation(-100, -1000, TimeSpan.FromSeconds(1)); ;

            DoubleAnimation adin = new DoubleAnimation(-2000, 0, TimeSpan.FromSeconds(1));
            DoubleAnimation adout = new DoubleAnimation(50, -2000, TimeSpan.FromSeconds(1));
            DoubleAnimation adhome2 = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            adhome2.RepeatBehavior = new RepeatBehavior(2);
            adhome2.AutoReverse = true;
            DoubleAnimation adback = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            adback.RepeatBehavior = new RepeatBehavior(2);
            adback.AutoReverse = true;
            DoubleAnimation addisbin = new DoubleAnimation(-1000, 0, TimeSpan.FromSeconds(1));
            DoubleAnimation addisbout = new DoubleAnimation(0, -1000, TimeSpan.FromSeconds(1));

            //home画面のアニメーション宣言
            Storyboard.SetTarget(had, hbutton1);
            Storyboard.SetTargetProperty(had, new PropertyPath(Button.OpacityProperty));
            sbhomead.Children.Add(had);
            hbutton1.Click += (sender, e) => { sbhomead.Begin(); homeflag = 0; };
            sbhomead.Completed += (sender, e) => { sbhomeout.Begin(); };

            Storyboard.SetTarget(hgame, hbutton2);
            Storyboard.SetTargetProperty(hgame, new PropertyPath(Button.OpacityProperty));
            sbhomegame.Children.Add(hgame);
            hbutton2.Click += (sender, e) => { sbhomegame.Begin(); homeflag = 1; };
            sbhomegame.Completed += (seder, e) => sbhomeout.Begin();

            Storyboard.SetTarget(homein, home);
            Storyboard.SetTargetProperty(homein, new PropertyPath(Canvas.RightProperty));
            sbhomein.Children.Add(homein);
            home.Loaded += (sender, e) => sbhomein.Begin();

            Storyboard.SetTarget(homeout, home);
            Storyboard.SetTargetProperty(homeout, new PropertyPath(Canvas.RightProperty));
            sbhomeout.Children.Add(homeout);

            sbhomeout.Completed += (sender, e) =>
            {
                home.Visibility = Visibility.Hidden;

                if (homeflag == 0)
                {
                    adlist.Visibility = Visibility.Visible;
                    sbadin.Begin();
                }
                else
                {
                    gamehome.Visibility = Visibility.Visible;
                    gamestart();
                }
            };

            //game画面の宣言
            gamehome.Click += (sender, e) =>
            {
                gameover();
                game_state = false;
                gamehome.Visibility = Visibility.Hidden;
                home.Visibility = Visibility.Visible;
                sbhomein.Begin();
            };

            //adlistのin-outの宣言

            Storyboard.SetTarget(ad1, adbutton1);
            Storyboard.SetTargetProperty(ad1, new PropertyPath(Button.OpacityProperty));
            sbad1.Children.Add(ad1);

            Storyboard.SetTarget(ad1in, adlist1);
            Storyboard.SetTargetProperty(ad1in, new PropertyPath(Canvas.LeftProperty));
            sbadin.Children.Add(ad1in);

            Storyboard.SetTarget(ad1in2, adlist1);
            Storyboard.SetTargetProperty(ad1in2, new PropertyPath(Canvas.TopProperty));
            sbadin.Children.Add(ad1in2);

            Storyboard.SetTarget(ad1out, adlist1);
            Storyboard.SetTargetProperty(ad1out, new PropertyPath(Canvas.LeftProperty));
            sbadout.Children.Add(ad1out);

            Storyboard.SetTarget(ad1out2, adlist1);
            Storyboard.SetTargetProperty(ad1out2, new PropertyPath(Canvas.TopProperty));
            sbadout.Children.Add(ad1out2);


            Storyboard.SetTarget(ad2, adbutton2);
            Storyboard.SetTargetProperty(ad2, new PropertyPath(Button.OpacityProperty));
            sbad2.Children.Add(ad2);

            Storyboard.SetTarget(ad2in, adlist2);
            Storyboard.SetTargetProperty(ad2in, new PropertyPath(Canvas.RightProperty));
            sbadin.Children.Add(ad2in);

            Storyboard.SetTarget(ad2in2, adlist2);
            Storyboard.SetTargetProperty(ad2in2, new PropertyPath(Canvas.TopProperty));
            sbadin.Children.Add(ad2in2);

            Storyboard.SetTarget(ad2out, adlist2);
            Storyboard.SetTargetProperty(ad2out, new PropertyPath(Canvas.RightProperty));
            sbadout.Children.Add(ad2out);

            Storyboard.SetTarget(ad2out2, adlist2);
            Storyboard.SetTargetProperty(ad2out2, new PropertyPath(Canvas.TopProperty));
            sbadout.Children.Add(ad2out2);


            Storyboard.SetTarget(ad3, adbutton3);
            Storyboard.SetTargetProperty(ad3, new PropertyPath(Button.OpacityProperty));
            sbad3.Children.Add(ad3);

            Storyboard.SetTarget(ad3in, adlist3);
            Storyboard.SetTargetProperty(ad3in, new PropertyPath(Canvas.LeftProperty));
            sbadin.Children.Add(ad3in);

            Storyboard.SetTarget(ad3in2, adlist3);
            Storyboard.SetTargetProperty(ad3in2, new PropertyPath(Canvas.BottomProperty));
            sbadin.Children.Add(ad3in2);

            Storyboard.SetTarget(ad3out, adlist3);
            Storyboard.SetTargetProperty(ad3out, new PropertyPath(Canvas.LeftProperty));
            sbadout.Children.Add(ad3out);

            Storyboard.SetTarget(ad3out2, adlist3);
            Storyboard.SetTargetProperty(ad3out2, new PropertyPath(Canvas.BottomProperty));
            sbadout.Children.Add(ad3out2);

            Storyboard.SetTarget(adhome, adbutton4);
            Storyboard.SetTargetProperty(adhome, new PropertyPath(Button.OpacityProperty));
            sbad4.Children.Add(adhome);

            Storyboard.SetTarget(adhomein, adlist4);
            Storyboard.SetTargetProperty(adhomein, new PropertyPath(Canvas.RightProperty));
            sbadin.Children.Add(adhomein);

            Storyboard.SetTarget(adhomein2, adlist4);
            Storyboard.SetTargetProperty(adhomein2, new PropertyPath(Canvas.BottomProperty));
            sbadin.Children.Add(adhomein2);

            Storyboard.SetTarget(adhomeout, adlist4);
            Storyboard.SetTargetProperty(adhomeout, new PropertyPath(Canvas.RightProperty));
            sbadout.Children.Add(adhomeout);

            Storyboard.SetTarget(adhomeout2, adlist4);
            Storyboard.SetTargetProperty(adhomeout2, new PropertyPath(Canvas.BottomProperty));
            sbadout.Children.Add(adhomeout2);

            //adlist時のハンドル

            adbutton1.Click += (sender, e) => { sbad1.Begin(); adflag = 0; adpic = 1; };
            sbad1.Completed += (sender, e) => sbadout.Begin();

            adbutton2.Click += (sender, e) => { sbad2.Begin(); adflag = 0; adpic = 2; };
            sbad2.Completed += (sender, e) => sbadout.Begin();

            adbutton3.Click += (sender, e) => { sbad3.Begin(); adflag = 0; adpic =3; };
            sbad3.Completed += (sender, e) => sbadout.Begin();

            adbutton4.Click += (sender, e) => { sbad4.Begin(); adflag = 1; };
            sbad4.Completed += (sender, e) => sbadout.Begin();

            sbadout.Completed += (sender, e) =>
            {
                adlist.Visibility = Visibility.Hidden;

                if (adpic == 1) adpic1.Visibility = Visibility.Visible;
                else if (adpic == 2) adpic2.Visibility = Visibility.Visible;
                else if (adpic == 3) adpic3.Visibility = Visibility.Visible;

                if (adflag == 0)
                {
                    addisplay.Visibility = Visibility.Visible;
                    sbaddisin.Begin();
                    sbaddisbin.Begin();
                }
                else
                {
                    home.Visibility = Visibility.Visible;
                    sbhomein.Begin();
                }
            };

            //addisplayのアニメーション宣言
            Storyboard.SetTarget(adin, adcv);
            Storyboard.SetTargetProperty(adin, new PropertyPath(Canvas.LeftProperty));
            sbaddisin.Children.Add(adin);

            Storyboard.SetTarget(adout, adcv);
            Storyboard.SetTargetProperty(adout, new PropertyPath(Canvas.LeftProperty));
            sbaddisout.Children.Add(adout);

            Storyboard.SetTarget(adback, backbutton);
            Storyboard.SetTargetProperty(adback, new PropertyPath(Button.OpacityProperty));
            sbaddisb1.Children.Add(adback);

            Storyboard.SetTarget(adhome, homebutton);
            Storyboard.SetTargetProperty(adhome, new PropertyPath(Button.OpacityProperty));
            sbaddisb2.Children.Add(adhome);

            Storyboard.SetTarget(addisbin, addisbutton);
            Storyboard.SetTargetProperty(addisbin, new PropertyPath(Canvas.RightProperty));
            sbaddisbin.Children.Add(addisbin);

            Storyboard.SetTarget(addisbout, addisbutton);
            Storyboard.SetTargetProperty(addisbout, new PropertyPath(Canvas.RightProperty));
            sbaddisbout.Children.Add(addisbout);

            //addisplay時のハンドル
            backbutton.Click += (sender, e) =>
            {
                adflag2 = 0;
                sbaddisb1.Begin();
            };
            sbaddisb1.Completed += (sender, e) =>
            {
                sbaddisout.Begin();
                sbaddisbout.Begin();
            };

            homebutton.Click += (sender, e) =>
            {
                adflag2 = 1;
                sbaddisb2.Begin();
            };
            sbaddisb2.Completed += (sender, e) =>
            {
                sbaddisout.Begin();
                sbaddisbout.Begin();
            };

            sbaddisout.Completed += (sender, e) =>
            {
                addisplay.Visibility = Visibility.Hidden;

                adpic1.Visibility = Visibility.Hidden;
                adpic2.Visibility = Visibility.Hidden;
                adpic3.Visibility = Visibility.Hidden;

                if (adflag2 == 0)
                {
                    adlist.Visibility = Visibility.Visible;
                    sbadin.Begin();
                }
                else
                {
                    home.Visibility = Visibility.Visible;
                    sbhomein.Begin();
                }
            };

            //時計表示
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            StartKinect(KinectSensor.KinectSensors[0]);

            ///////////////////////////////////////////////////////
            //アニメーション関連
            sb = new Storyboard();
            sb2 = new Storyboard();
            sb3 = new Storyboard();
            sb4 = new Storyboard();
            string stCurrentDir = System.IO.Directory.GetCurrentDirectory();
            string music = "\\sound1.mp3";
            string sounduri = stCurrentDir + music;


            DoubleAnimation ani = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            DoubleAnimation ani2 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            DoubleAnimation ani3 = new DoubleAnimation(2000, 50, TimeSpan.FromSeconds(1));
            DoubleAnimation ani4 = new DoubleAnimation(50, -2000, TimeSpan.FromSeconds(1));


            Storyboard.SetTarget(ani3, cv1);
            Storyboard.SetTargetProperty(ani3, new PropertyPath(Canvas.LeftProperty));
            sb3.Children.Add(ani3);

            Storyboard.SetTarget(ani4, cv1);
            Storyboard.SetTargetProperty(ani4, new PropertyPath(Canvas.LeftProperty));
            sb4.Children.Add(ani4);
            ///////////////////////////////////////////////////////

        }

        //時計
        void timer_Tick(object sender, EventArgs e)
        {
            this.textBlock1.Text = DateTime.Now.ToString("HH:mm:ss");

            //game
            if (game_state == true)
            {
                //乱数を取得する
                Random rnd = new Random();

                timeUp++;
                time++;


                TopPosition = TopPosition + 25;
                TopPosition2 = TopPosition2 + 30;
                TopPosition3 = TopPosition3 + 60;
                TopPosition4 = TopPosition4 + 20;
                TopPosition5 = TopPosition5 + 45;
                if (peach_flag == 1)
                {
                    TopPosition6 = TopPosition6 + 100;
                }

                Random rnd1 = new Random();
                int randomN5 = rnd1.Next(500);


                Canvas.SetTop(peach1, TopPosition);
                Canvas.SetTop(peach2, TopPosition2);
                Canvas.SetTop(peach3, TopPosition3);
                Canvas.SetTop(peach4, TopPosition4);
                Canvas.SetTop(peach5, TopPosition5);
                Canvas.SetTop(peachA, TopPosition6);

                Canvas.SetLeft(peach1, LeftPosition);
                Canvas.SetLeft(peach2, LeftPosition2);
                Canvas.SetLeft(peach3, LeftPosition3);
                Canvas.SetLeft(peach4, LeftPosition4);
                Canvas.SetLeft(peach5, LeftPosition5);

                if (TopPosition > Canvas1.ActualHeight)
                {
                    TopPosition = 0;
                }
                if (TopPosition2 > Canvas1.ActualHeight)
                {
                    TopPosition2 = 0;
                }
                if (TopPosition3 > Canvas1.ActualHeight)
                {
                    TopPosition3 = 0;
                }
                if (TopPosition4 > Canvas1.ActualHeight)
                {
                    TopPosition4 = 0;
                }
                if (TopPosition5 > Canvas1.ActualHeight)
                {
                    TopPosition5 = 0;
                }
                if (TopPosition6 > Canvas1.ActualHeight)
                {
                    TopPosition6 = 0;

                    LeftPosition6 = randomN5;
                    Canvas.SetLeft(peachA, LeftPosition6);
                    peach_flag = 0;
                }

                if (timeUp > 7 && timeUp <= 10)
                {
                    peachB.Visibility = Visibility.Visible;
                }
                else
                {
                    peachB.Visibility = Visibility.Hidden;
                }

                if (time % 10 == 0)
                {
                    TopPosition6 = -50;
                    peachA.Visibility = Visibility.Visible;
                    peach_flag = 1;
                }
            }
        }

        /// <summary>
        /// RGBカメラ、スケルトンのフレーム更新イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            try
            {
                KinectSensor kinect = sender as KinectSensor;

                using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
                {
                    if (colorFrame != null)
                    {
                        byte[] colorPixel = new byte[colorFrame.PixelDataLength];
                        colorFrame.CopyPixelDataTo(colorPixel);
                        imageRgb.Source = BitmapSource.Create(colorFrame.Width, colorFrame.Height, 96, 96,
                            PixelFormats.Bgr32, null, colorPixel, colorFrame.Width * colorFrame.BytesPerPixel);
                    }
                }

                using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
                {
                    if (skeletonFrame != null)
                    {
                        // トラッキングされているスケルトンのジョイントを描画する
                        Skeleton skeleton = skeletonFrame.GetFirstTrackedSkeleton();

                        if ((skeleton != null) && (skeleton.TrackingState == SkeletonTrackingState.Tracked))
                        {
                            Joint hand = skeleton.Joints[JointType.HandRight];
                            if (hand.TrackingState == JointTrackingState.Tracked)
                            {
                                ImageSource source = imageRgb.Source;
                                DrawingVisual drawingVisual = new DrawingVisual();

                                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                                {
                                    //バイト列をビットマップに展開
                                    //描画可能なビットマップを作る
                                    drawingContext.DrawImage(imageRgb.Source,
                                    new Rect(0, 0, source.Width, source.Height));

                                    // 手の位置に円を描画
                                    //DrawSkeletonPoint(drawingContext, hand);
                                }

                                // 描画可能なビットマップを作る
                                // http://stackoverflow.com/questions/831860/generate-bitmapsource-from-uielement
                                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)source.Width,
                                    (int)source.Height, 96, 96, PixelFormats.Default);
                                bitmap.Render(drawingVisual);

                                imageRgb.Source = bitmap;

                                // Frame中の手の位置をディスプレイの位置に対応付ける
                                ColorImagePoint point = kinect.MapSkeletonPointToColor(hand.Position,        // スケルトン座標 → RGB画像座標
                                    kinect.ColorStream.Format);
                                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[0];   // メインディスプレイの情報を取得
                                point.X = (point.X * screen.Bounds.Width) / kinect.ColorStream.FrameWidth;
                                point.Y = (point.Y * screen.Bounds.Height) / kinect.ColorStream.FrameHeight;

                                // マウスカーソルの移動
                                SendInput.MouseMove(point.X, point.Y, screen);

                                // クリック動作
                                //一定時間そこに手を置いていたら
                                if (IsClicked(skeletonFrame, point))
                                {
                                    SendInput.LeftClick();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Kinectの動作を開始する
        /// </summary>
        /// <param name="kin"></param>
        private void StartKinect(KinectSensor kin)
        {
            try
            {
                if (KinectSensor.KinectSensors.Count == 0)
                {
                    throw new Exception("Kinectが接続されていません");
                }

                kinect = kin;
                kinect.ColorStream.Enable();
                kinect.SkeletonStream.Enable(new TransformSmoothParameters()
                {
                    Smoothing = 0.7f,
                    Correction = 0.3f,
                    Prediction = 0.4f,
                    JitterRadius = 0.10f,
                    MaxDeviationRadius = 0.5f,
                });

                kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);

                kinect.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        /// <summary>
        /// Kinectの動作を停止する
        /// </summary>
        /// <param name="kinect"></param>
        private void StopKinect(KinectSensor kinect)
        {
            if (kinect != null)
            {
                if (kinect.IsRunning)
                {
                    kinect.AllFramesReady -= kinect_AllFramesReady;

                    kinect.Stop();
                    kinect.Dispose();
                }
            }
        }

        /// <summary>
        /// クリック動作を行ったのかチェック
        /// </summary>
        /// <param name="skeletonFrame"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool IsClicked(SkeletonFrame skeletonFrame, ColorImagePoint point)
        {
            return IsSteady(skeletonFrame, point);
        }

        /// <summary>
        /// 座標管理用構造体
        /// </summary>
        struct FramePoint
        {
            public ColorImagePoint Point;
            public long TimeStamp;
        }

        int milliseconds = 1000;        // 認識するまでの停止時間の設定
        int threshold = 10;             // 座標の変化量の閾値

        // 基点となるポイント。
        // この座標からあまり動かない場合 Steady状態であると認識する。
        FramePoint basePoint = new FramePoint();

        /// <summary>
        /// 停止状態にあるかチェックする
        /// </summary>
        /// <param name="skeletonFrame"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        bool IsSteady(SkeletonFrame skeletonFrame, ColorImagePoint point)
        {
            var currentPoint = new FramePoint()
            {
                Point = point,
                TimeStamp = skeletonFrame.Timestamp,
            };

            // milliseconds時間経過したら steady
            if ((currentPoint.TimeStamp - basePoint.TimeStamp) > milliseconds)
            {
                basePoint = currentPoint;
                return true;
            }

            // 座標の変化量がthreshold以上ならば、basePointを更新して初めから計測
            if (Math.Abs(currentPoint.Point.X - basePoint.Point.X) > threshold
            || Math.Abs(currentPoint.Point.Y - basePoint.Point.Y) > threshold)
            {

                // 座標が動いたので基点を動いた位置にずらして、最初から計測
                basePoint = currentPoint;
            }

            return false;
        }

        /// <summary>
        /// jointの座標に円を描く
        /// </summary>
        /// <param name="drawingContext"></param>
        /// <param name="joint"></param>
        private void DrawSkeletonPoint(DrawingContext drawingContext, Joint joint)
        {
            if (joint.TrackingState != JointTrackingState.Tracked)
            {
                return;
            }

            // 円を書く
            /*
            ColorImagePoint point = kinect.MapSkeletonPointToColor(joint.Position,
            kinect.ColorStream.Format);
            drawingContext.DrawEllipse(new SolidColorBrush(Colors.Red),
                new Pen(Brushes.Red, 1), new Point(point.X, point.Y), R, R);
             */
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopKinect(KinectSensor.KinectSensors[0]);
        }


        private void gamestart()
        {
            //ゲーム開始
            game_state = true;
            total_point = 0;

            //桃、スコア、ホームボタンの表示
            peach1.Visibility = Visibility.Visible;
            peach2.Visibility = Visibility.Visible;
            peach3.Visibility = Visibility.Visible;
            peach4.Visibility = Visibility.Visible;
            peach5.Visibility = Visibility.Visible;
            score_board.Visibility = Visibility.Visible;
            kago.Visibility = Visibility.Visible;
        }

        private void gameover()
        {
            //ゲーム終了
            game_state = false;

            //桃、スコア、ホームボタンの表示
            peach1.Visibility = Visibility.Hidden;
            peach2.Visibility = Visibility.Hidden;
            peach3.Visibility = Visibility.Hidden;
            peach4.Visibility = Visibility.Hidden;
            peach5.Visibility = Visibility.Hidden;
            peachA.Visibility = Visibility.Hidden;
            score_board.Visibility = Visibility.Hidden;
            kago.Visibility = Visibility.Hidden;
        }


  
        private void peach1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TopPosition = 0;

            //ポイントのカウント
            total_point++;

            //socre_boardの表示文字変更
            //total_pointを文字で表示
            score_board.Text = "score : " + total_point;

            //次のｙ座標をランダムで決める
            Random rnd = new Random();
            int randomN1 = rnd.Next(1000);
            LeftPosition = randomN1;

            TopPosition = -25;
        }

        private void peach2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TopPosition2 = 0;

            //ポイントのカウント
            total_point++;

            //socre_boardの表示文字変更
            //total_pointを文字で表示
            score_board.Text = "score : " + total_point;

            //次のｙ座標をランダムで決める
            Random rnd = new Random();
            int randomN2 = rnd.Next(1000);
            LeftPosition2 = randomN2;


            TopPosition2 = -30;
        }

        private void peach3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TopPosition3 = 0;

            //ポイントのカウント
            total_point++;

            //socre_boardの表示文字変更
            //total_pointを文字で表示
            score_board.Text = "score : " + total_point;
            //score_board.Foreground = Color.Red;

            //次のｙ座標をランダムで決める
            Random rnd = new Random();
            int randomN3 = rnd.Next(1000);
            LeftPosition3 = randomN3;


            TopPosition3 = -60;
        }

        private void peach4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TopPosition4 = 0;

            //ポイントのカウント
            total_point++;

            //socre_boardの表示文字変更
            //total_pointを文字で表示
            score_board.Text = "score : " + total_point;
            //score_board.Foreground = Color.Red;

            //次のｙ座標をランダムで決める
            Random rnd = new Random();
            int randomN4 = rnd.Next(1000);
            LeftPosition4 = randomN4;


            TopPosition4 = -60;
        }

        private void peach5_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TopPosition5 = 0;

            //ポイントのカウント
            total_point++;

            //socre_boardの表示文字変更
            //total_pointを文字で表示
            score_board.Text = "score : " + total_point;
            //score_board.Foreground = Color.Red;

            //次のｙ座標をランダムで決める
            Random rnd = new Random();
            int randomN5 = rnd.Next(1000);
            LeftPosition5 = randomN5;


            TopPosition5 = -45;
        }

        private void peachA_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            total_point = total_point + 5;
            score_board.Text = "score : " + total_point;
            TopPosition5 = -45;

        }

        private void peachB_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            total_point = total_point + 2;
            score_board.Text = "score : " + total_point;
        }

    }

    /// <summary>
    /// 初めにトラッキングしたスケルトンを取得する拡張メソッド
    /// </summary>
    public static class SkeletonExtensions
    {
        public static Skeleton GetFirstTrackedSkeleton(this SkeletonFrame skeletonFrame)
        {
            Skeleton[] skeleton = new Skeleton[skeletonFrame.SkeletonArrayLength];
            skeletonFrame.CopySkeletonDataTo(skeleton);
            return (from s in skeleton
                    where s.TrackingState == SkeletonTrackingState.Tracked
                    select s).FirstOrDefault();
        }
    }
}
