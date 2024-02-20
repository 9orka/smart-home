using System;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Speech;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Media;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Collections.Generic;
using AudioSwitcher.AudioApi.CoreAudio;

namespace smarthouse
{
    public partial class Form2 : Form
    {
        private CultureInfo culture;
        private SpeechRecognitionEngine sre;
        VideoCaptureDevice frame;
        FilterInfoCollection Devices;

        bool light = false;
        bool music = false;
        bool doors = false;
        bool camera = false;
        int temperature = 18;
        int t=0;
        SoundPlayer player = new SoundPlayer();
        List<string> tracks = new List<string>{
            @"C:\Users\igoro\OneDrive\Рабочий стол\smarthouse\smarthouse\Resources\music\bethoven.wav",
        @"C:\Users\igoro\OneDrive\Рабочий стол\smarthouse\smarthouse\Resources\music\amoreplast.wav",
        @"C:\Users\igoro\OneDrive\Рабочий стол\smarthouse\smarthouse\Resources\music\lamenthe.wav",
        @"C:\Users\igoro\OneDrive\Рабочий стол\smarthouse\smarthouse\Resources\music\djai.wav",
        @"C:\Users\igoro\OneDrive\Рабочий стол\smarthouse\smarthouse\Resources\music\megustas.wav"
        };
        public Form2()
        {
            InitializeComponent();

        }
       

        System.Speech.Synthesis.SpeechSynthesizer synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
        // synthesizer.Volume = 100;
        //synthesizer.Rate = 3;

        #region camera
        public void Camera()
        {
            if (camera)
            {
                camera = false;
                frame.Stop();
                pictureBox3.Image = null;
                pictureBox12.Visible = false;
            }
            else
            {
                camera = true;
                Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                frame = new VideoCaptureDevice(Devices[0].MonikerString);
                frame.NewFrame += new AForge.Video.NewFrameEventHandler(NewFrame_event);
                frame.Start();
                pictureBox12.Visible = true;
            }
        }
       
       public void NewFrame_event(object send, NewFrameEventArgs e)
        {
            try
            {
               
                pictureBox3.Image = (Image)e.Frame.Clone();
            }
            catch (Exception ex) { }
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Camera();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Camera();
        }
        #endregion

        #region Light
        public void Light()
        {
            if (light)
            {
                light = false;
                pictureBox9.Visible = false;
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
            }
            else
            {
                light = true;
                pictureBox9.Visible = true;
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
            }

        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Light();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Light();
        }
        #endregion

        #region Music
        public void Music()
        {
            player = new SoundPlayer(tracks[t]);
            if (music)
            {
                music = false;
                
                pictureBox10.Visible = false;
                pictureBox24.Visible = false;
                pictureBox23.Visible = true;
                player.Stop();
                synthesizer.Speak("Уже Насладились музыкой?");


            }
            else
            {
                music = true;
                pictureBox10.Visible = true;
                pictureBox23.Visible = false;
                pictureBox24.Visible = true;
                synthesizer.Speak("Музыка хорошее средство от депрессии");
                player.Play();
            }

        }
        public void Volume(string value)
        {
            if (value == "+")
            {
                CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                defaultPlaybackDevice.Volume += 10;
                trackBar1.Value += 1;
            }
            else if (value == "-")
            {
                CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                defaultPlaybackDevice.Volume -= 10;
                trackBar1.Value -= 1;
            }
        }

        public void Compositions(string value)
        {
            if (music)
            {
                if (value == "+")
                {
                    trackBar1.Value += 1;
                    player.Stop();
                    if (t == 4)
                    {
                        t = 0;
                    }
                    else
                    {
                        t++;
                    }
                    player = new SoundPlayer(tracks[t]);
                    player.Play();
                }
                else if (value == "-")
                {
                    trackBar1.Value -= 1;
                    player.Stop();
                    if (t == 0)
                    {
                        t = 4;
                    }
                    else
                    {
                        t--;
                    }
                    player = new SoundPlayer(tracks[t]);
                    player.Play();
                }
            }
            else
            {
                synthesizer.Speak("Пожалуйста, включите музыку.");
            }
        }
        private void pictureBox23_Click(object sender, EventArgs e)
        {
            Music();

        }
        private void pictureBox24_Click(object sender, EventArgs e)
        {
            Music();
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Music();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Music();
        }

        private void pictureBox19_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox21.Visible = true;
        }

        private void pictureBox21_MouseLeave(object sender, EventArgs e)
        {
            pictureBox21.Visible = false;
        }

        private void pictureBox20_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox22.Visible = true;
        }

        private void pictureBox22_MouseLeave(object sender, EventArgs e)
        {
            pictureBox22.Visible = false;
        }

      
        public void DefaultVolume()
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = 10;
            trackBar1.Value = 1;
        }
        #endregion

        #region Doors
        public void Doors()
        {
            if (doors)
            {
                doors = false;
                pictureBox11.Visible = false;
                pictureBox17.Visible = false;

            }
            else if (!light && !doors)
            {
                synthesizer.Speak("Сначала включите свет");
            }
            else if(light)
            {

                doors = true;
                pictureBox11.Visible = true;
                pictureBox17.Visible = true;    
            }
        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Doors();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Doors();
        }
        #endregion

        #region Temperature
        void TempPlus()
        {
            if (temperature <= 50)
            {
                temperature += 1;
                pictureBox16.Enabled = true;

            }
               
            else
            {
                synthesizer.Speak("Температура слишком высокая");
                pictureBox15.Enabled=false;
            }
                
                
                
            Temp(temperature);
        }
        void TempMinus()
        {
            if (temperature >= -10)
            {
                temperature -= 1;
                pictureBox15.Enabled = true;
            }
                
            else
            {
                synthesizer.Speak("Температура слишком низкая");
                pictureBox16.Enabled = false;
                
            }
                
            Temp(temperature);
        }
        void Temp(int temp)
        {
            label1.Text = Convert.ToString(temp + "°С");
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            TempMinus();
        }
        private void pictureBox15_Click(object sender, EventArgs e)
        {
            TempPlus();
        }


        private void pictureBox14_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox15.Visible = true;
        }

        private void pictureBox15_MouseLeave(object sender, EventArgs e)
        {
            pictureBox15.Visible = false;
        }
        private void pictureBox16_MouseLeave(object sender, EventArgs e)
        {
            pictureBox16.Visible = false;
        }

        private void pictureBox13_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox16.Visible = true;
        }
        #endregion
        private void Form2_Load(object sender, EventArgs e)
        {
            
            try
            {
                culture = new CultureInfo("ru-RU");
                sre = new SpeechRecognitionEngine(culture);


                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);


                sre.SetInputToDefaultAudioDevice();


                sre.LoadGrammar(CreateSampleGrammar1());
                sre.LoadGrammar(CreateSampleGrammar2());
                sre.LoadGrammar(CreateSampleGrammar3());
                sre.LoadGrammar(CreateSampleGrammar4());
                sre.LoadGrammar(CreateSampleGrammar5());
                sre.LoadGrammar(CreateSampleGrammar6());
                sre.LoadGrammar(CreateSampleGrammar7());
                sre.LoadGrammar(CreateSampleGrammar8());

                // start recognition
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private Choices CreateSampleChoices()
        {
            var val1 = new SemanticResultValue("свет", "light");
            var val2 = new SemanticResultValue("климат-контроль", "climate");
            var val3 = new SemanticResultValue("музыку", "music");
            var val4 = new SemanticResultValue("видеонаблюдение", "security");
            var val5 = new SemanticResultValue("двери", "doors");
            var val6 = new SemanticResultValue("температуру", "temper");
            var val7 = new SemanticResultValue("композиция", "composition");
            var val8 = new SemanticResultValue("громкость", "volume");

            return new Choices(val1, val2, val3, val4, val5, val6, val7, val8);
        }

        #region grammarbuilders7
        private Grammar CreateSampleGrammar1()
        {
            var function = CreateSampleChoices();
            var grammarBuilder = new GrammarBuilder("включить", SubsetMatchingMode.SubsequenceContentRequired);
            // grammarBuilder.Append("");
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("on", function));
            return new Grammar(grammarBuilder);
        }

        private Grammar CreateSampleGrammar2()
        {
            var function = CreateSampleChoices();
            var grammarBuilder = new GrammarBuilder("выключить", SubsetMatchingMode.SubsequenceContentRequired);
            //grammarBuilder.Append("");
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("off", function));
            return new Grammar(grammarBuilder);
        }
        private Grammar CreateSampleGrammar3()
        {
            var function = CreateSampleChoices();
            var grammarBuilder = new GrammarBuilder("открыть", SubsetMatchingMode.SubsequenceContentRequired);
            // grammarBuilder.Append("");
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("open", function));
            return new Grammar(grammarBuilder);
        }
        private Grammar CreateSampleGrammar4()
        {
            var function = CreateSampleChoices();

            var grammarBuilder = new GrammarBuilder("закрыть", SubsetMatchingMode.SubsequenceContentRequired);
            // grammarBuilder.Append("");
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("close", function));
            return new Grammar(grammarBuilder);
        }
        private Grammar CreateSampleGrammar5()
        {
            var function = CreateSampleChoices();

            var grammarBuilder = new GrammarBuilder("увеличить", SubsetMatchingMode.SubsequenceContentRequired);
            // grammarBuilder.Append("");
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("plus", function));
            return new Grammar(grammarBuilder);
        }
        private Grammar CreateSampleGrammar6()
        {
            var function = CreateSampleChoices();

            var grammarBuilder = new GrammarBuilder("уменьшить", SubsetMatchingMode.SubsequenceContentRequired);
            // grammarBuilder.Append("");
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("minus", function));
            return new Grammar(grammarBuilder);
        }

        private Grammar CreateSampleGrammar7()
        {
            var function = CreateSampleChoices();

            var grammarBuilder = new GrammarBuilder("следующая", SubsetMatchingMode.SubsequenceContentRequired);
            // grammarBuilder.Append("");
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("next", function));
            return new Grammar(grammarBuilder);
        }

        private Grammar CreateSampleGrammar8()
        {
            var function = CreateSampleChoices();

            var grammarBuilder = new GrammarBuilder("предыдущая", SubsetMatchingMode.SubsequenceContentRequired);
            // grammarBuilder.Append("");
            grammarBuilder.Culture = culture;
            grammarBuilder.Append(new SemanticResultKey("back", function));
            return new Grammar(grammarBuilder);
        }
        #endregion

        /*private void sr_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            AppendLine("Распознавание речи отклонено: " + e.Result.Text);
        }

        private void sr_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            AppendLine("Возможный текст: " + e.Result.Text + " (" + e.Result.Confidence + ")");
        }

        private void sr_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            AppendLine("Распознование завершено: " + e.Result.Text);
        }
        */

        private void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.5)
            {
                //AppendLine("\t" + "Распознанная речь:");

               // AppendLine(e.Result.Text + " (" + e.Result.Confidence + ")");
               
                foreach (var s in e.Result.Semantics)
                {
                    var value = (string)s.Value.Value;

                    switch (s.Key)
                    {
                        case "on":
                            switch (value)
                            {
                                case "light":
                                    synthesizer.Speak("Момент, сейчас включу");
                                    Light();
                                    break;
                                case "climate":
                                    synthesizer.Speak("Включаю");
                                    break;
                                case "music":
                                    Music();
                                    break;
                                case "security":
                                    synthesizer.Speak("Безопасность превыше всего");
                                    Camera();
                                    
                                    break;
                            }
                            break;
                        case "off":
                            switch (value)
                            {
                                case "light":
                                    synthesizer.Speak("Секунду, сейчас выключу");
                                    Light();
                                   break;
                                case "climate":
                                    synthesizer.Speak("Уже выключаю");
                                    break;
                                case "music":
                                    Music();
                                    break;
                                case "security":
                                    synthesizer.Speak("Наблюдение отключено");
                                    Camera();
                                    break;
                            }
                            break;
                        case "open":
                            switch (value)
                            {
                                case "doors":
                                    synthesizer.Speak("Открываю двери");
                                    Doors();
                                    break;
                            }
                            break;
                        case "close":
                            switch (value)
                            {
                                case "doors":
                                    synthesizer.Speak("Двери закрыты");
                                    Doors();
                                    break;
                            }
                            break;
                        case "plus":
                            switch (value)
                            {
                                case "temper":
                                    TempPlus();
                                    break;
                                case "volume":
                                    Volume("+");
                                    break;
                            }
                            break;
                        case "minus":
                            switch (value)
                            {
                                case "temper":
                                    TempMinus();
                                    break;
                                case "volume":
                                    Volume("-");
                                    break;
                            }
                            break;
                        case "next":
                            switch(value)
                            {
                                case "composition":
                                    Compositions("+");
                                     break;
                            }
                            break;
                        case "back":
                            switch (value)
                            {
                                case "composition":
                                    Compositions("-");
                                    break;
                            }
                            break;
                    }
                }
            }

        }


       /* private void AppendLine(string text)
        {
            richTextBox1.AppendText(text + Environment.NewLine);
            richTextBox1.ScrollToCaret();
        }*/
    

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 newForm = new Form3();
            newForm.Show();
        }

        string[] paths, files;

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            player.Stop();
            if(t == 4)
            {
                t = 0;
            }
            else
            {
                t++;
            }
            player = new SoundPlayer(tracks[t]);
            player.Play();
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            player.Stop();
            if (t == 0)
            {
                t = 4;
            }
            else
            {
                t--;
            }
            player = new SoundPlayer(tracks[t]);
            player.Play();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = trackBar1.Value * 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
