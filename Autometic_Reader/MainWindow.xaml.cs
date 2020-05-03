using System;
using System.Windows;
using System.Windows.Documents;
using System.Speech.Synthesis;
using System.Media;
using Microsoft.Win32;
using System.IO;
using System.Windows.Threading;
using System.Diagnostics;


namespace Autometic_Reader
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// Autometic Reader .10 is open source software under GNU v2 license
	/// Developer MD. Hasibur Rashid from Software-Art
	/// Copyright by Software-Art (c) 2012-13
	/// 

	public partial class MainWindow : Window
	{
		string _readtext;
		readonly SpeechSynthesizer _voice = new SpeechSynthesizer();
		readonly SoundPlayer _sound = new SoundPlayer();


		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			CheckInstance();
		}


		public MainWindow()
		{

			this.InitializeComponent();
			Clock(); DefaultLoad();

			splash.Visibility = System.Windows.Visibility.Visible;
			Read.Visibility = System.Windows.Visibility.Hidden;
			About.Visibility = System.Windows.Visibility.Hidden;
			settings.Visibility = System.Windows.Visibility.Hidden;

			play_button.Visibility = System.Windows.Visibility.Visible;
			Pause_button.Visibility = System.Windows.Visibility.Hidden;
			resume_button.Visibility = System.Windows.Visibility.Hidden;
			StartingMusic();

			_voice.SpeakAsync("Welcome to software art Automatic Reader");
		}


		public void StartingMusic()
		{
			try
			{
				_sound.SoundLocation = "Resources\\startup.wav";
				_sound.PlaySync();
			}
			catch (Exception)
			{
				MessageBox.Show("File missing!", " Automatic Reader", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
		}



		// At a time one application run=======================================================


		private static void CheckInstance()
		{
			Process[] thisnameprocesslist;
			string modulename, processname;
			Process p = Process.GetCurrentProcess();
			modulename = p.MainModule.ModuleName.ToString();
			processname = System.IO.Path.GetFileNameWithoutExtension(modulename);
			thisnameprocesslist = Process.GetProcessesByName(processname);
			if (thisnameprocesslist.Length > 1)
			{
				MessageBox.Show("Instance of this application is already running.", "Automatic Reader", MessageBoxButton.OK
					, MessageBoxImage.Stop);
				Application.Current.Shutdown();
			}
		}


		public void Clock()
		{

			var timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
			{
				this.lableClock.Content = DateTime.Now.ToString("h:mm:ss");
				this.ampm.Text = DateTime.Now.ToString("tt");	//("HH:mm:ss tt"); //for AM PM HH=24 hours
				this.dateLabel.Text = DateTime.Now.ToLongDateString();
			}, this.Dispatcher);
		}


		public void DefaultLoad()
		{
			this.Volume_slider.Value = Properties.Settings.Default.defaultVolume;
			this.Rate_slider.Value = Properties.Settings.Default.defaultRate;
			this.VoiceChange_ComboBox.SelectedIndex = Properties.Settings.Default.readerVoice;
		}


		//start================================================================================================


		private void BaseRectangleMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void SoftwareartMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("http://www.facebook.com/pages/Software-Art/124544400963976");
			}
			catch (Exception)
			{
				MessageBox.Show("Sorry, link doesn't found.", "Reader", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
		}

		private void FacebookMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("https://www.facebook.com/hasib.cse.pstu");
			}
			catch (Exception)
			{

				MessageBox.Show("Sorry, link doesn't found.", "Reader", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
		}

		private void MinimizeButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Minimized;
		}


		private void ExitMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				this.Close();
				_sound.SoundLocation = "Resources\\Exit.wav";
				_sound.PlaySync();
			}
			catch (Exception)
			{
				MessageBox.Show("File missing!", "Automatic Reader", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
		}




		//Splash window =============================================================================================


		private void SplashReadButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			splash.Visibility = System.Windows.Visibility.Hidden;
			Read.Visibility = System.Windows.Visibility.Visible;
			About.Visibility = System.Windows.Visibility.Hidden;
			settings.Visibility = System.Windows.Visibility.Hidden;

		}

		private void SplashSettingsButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			splash.Visibility = System.Windows.Visibility.Hidden;
			Read.Visibility = System.Windows.Visibility.Hidden;
			About.Visibility = System.Windows.Visibility.Hidden;
			settings.Visibility = System.Windows.Visibility.Visible;

		}

		private void SplashAboutButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			splash.Visibility = System.Windows.Visibility.Hidden;
			Read.Visibility = System.Windows.Visibility.Hidden;
			About.Visibility = System.Windows.Visibility.Visible;
			settings.Visibility = System.Windows.Visibility.Hidden;

		}


   
        
        //Read window ====================================================================================================


		private void BackButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{

            splash.Visibility = System.Windows.Visibility.Visible;
            Read.Visibility = System.Windows.Visibility.Hidden;
            About.Visibility = System.Windows.Visibility.Hidden;
            settings.Visibility = System.Windows.Visibility.Hidden;

            play_button.Visibility = System.Windows.Visibility.Visible;
            Pause_button.Visibility = System.Windows.Visibility.Hidden;
            resume_button.Visibility = System.Windows.Visibility.Hidden;

               // _voice.SpeakAsyncCancelAll();
		   
            

		}

		private void LoadFileClick(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{

				var ofd = new OpenFileDialog();

				ofd.CheckFileExists = true;
				ofd.CheckPathExists = true;
				ofd.DefaultExt = "txt";
				ofd.DereferenceLinks = true;
				ofd.Filter = "Text files (*.txt)|*.txt|" + "RTF files (*.rtf)|*.rtf|" + "Works 6 and 7 (*.wps)|*.wps|" +
								  "Windows Write (*.wri)|*.wri|" + "WordPerfect document (*.wpd)|*.wpd";
				ofd.Multiselect = false;
				ofd.RestoreDirectory = true;
				//ofd.ShowHelp = true;
				ofd.ShowReadOnly = false;
				ofd.Title = "Select a file";
				ofd.ValidateNames = true;

				if (ofd.ShowDialog() == true)
				{
					RichTextBox.Document.Blocks.Clear();
					var sr = new StreamReader(ofd.OpenFile());
					String textFromFile = sr.ReadToEnd();
					RichTextBox.Document.Blocks.Add(new Paragraph(new Run("" + textFromFile)));
				}

			}
			catch (Exception)
			{
				MessageBox.Show("Can not open this file", "Automatic Reader", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void PlayButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			RichTextBox.SelectAll();
			_readtext = RichTextBox.Selection.Text;

			try
			{

				switch (VoiceChange_ComboBox.SelectedIndex)
				{
					case 0:
						_voice.SelectVoiceByHints(VoiceGender.Male); break;

					case 1:
						_voice.SelectVoiceByHints(VoiceGender.Female); break;

				}

				_voice.Volume = (int)Volume_slider.Value;
				_voice.Rate = (int)Rate_slider.Value;
				_voice.SpeakAsync(_readtext);

				play_button.Visibility = System.Windows.Visibility.Hidden;
				Pause_button.Visibility = System.Windows.Visibility.Visible;
				resume_button.Visibility = System.Windows.Visibility.Hidden;

				_voice.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(SpeakCompleted);

			}
			catch (Exception)
			{
				MessageBox.Show("Speak error", "Automatic Reader", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}


		private void SpeakCompleted(object sender, SpeakCompletedEventArgs e)
		{

			play_button.Visibility = System.Windows.Visibility.Visible;
			Pause_button.Visibility = System.Windows.Visibility.Hidden;
			resume_button.Visibility = System.Windows.Visibility.Hidden;
		}


		private void PauseButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
		    try
		    {
                _voice.Pause();
                play_button.Visibility = System.Windows.Visibility.Hidden;
                Pause_button.Visibility = System.Windows.Visibility.Hidden;
                resume_button.Visibility = System.Windows.Visibility.Visible;
		
		    }
		    catch (Exception)
		    {
                MessageBox.Show("Something rong on application, please restart the application.", "Automatic Reader",
                                MessageBoxButton.OK, MessageBoxImage.Information);
		        throw;
		    }
			}

		private void ResumeButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
		    try
		    {
                _voice.Resume();
                play_button.Visibility = System.Windows.Visibility.Hidden;
                Pause_button.Visibility = System.Windows.Visibility.Visible;
                resume_button.Visibility = System.Windows.Visibility.Hidden;
		    }
		    catch (Exception)
		    {
		        MessageBox.Show("Something rong on application, please restart the application.", "Automatic Reader",
		                        MessageBoxButton.OK, MessageBoxImage.Information);
		        throw;
		    }
            
		}


		private void StopButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
		    try
		    {
                _voice.SpeakAsyncCancelAll();
                play_button.Visibility = System.Windows.Visibility.Visible;
                Pause_button.Visibility = System.Windows.Visibility.Hidden;
                resume_button.Visibility = System.Windows.Visibility.Hidden;

		    }
		    catch (Exception)
		    {
                MessageBox.Show("Something rong on application, please restart the application.", "Automatic Reader",
                                MessageBoxButton.OK, MessageBoxImage.Information);
		        throw;
		    }
		 }

		private void RecordeButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			FileStream fs = null; var voice1 = new SpeechSynthesizer();

			try
			{

				var sdf = new SaveFileDialog
							  {
								  Filter = "All files (*.*)|*.*|wav files (*.wav)|*.wav",
								  Title = "Save Text to Audio",
								  FilterIndex = 2,
								  RestoreDirectory = true
							  };
				//Nullable<bool> result = sdf.ShowDialog();

				if (sdf.ShowDialog() == true)
				{
					voice1.SpeakAsyncCancelAll();
					RichTextBox.SelectAll();
					String readtext1 = RichTextBox.Selection.Text;
					fs = new FileStream(sdf.FileName, FileMode.Create, FileAccess.Write);
					voice1.SetOutputToWaveStream(fs);
					// voice.SetOutputToAudioStream();
					voice1.Speak(readtext1);


				}
			}


			finally
			{

				if (fs != null)
				{
					fs.Flush(); fs.Close(); Console.Beep();
					_voice.Speak("Recording Complete");

					//voice.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(RecodingCompleted); 
				}

			}

		}

		
        
        
       
        
        /* private void RecodingProcess(object sender,SpeakProgressEventArgs e) {

			  voice.SpeakAsync(""+e.UserState);
 
		  }



			private void RecodingCompleted(object sender, SpeakCompletedEventArgs e)
			{
	  
				voice.SpeakAsync("Recording complete.");
			   voice.SetOutputToNull();
				Console.Beep();
			   Console.WriteLine("Recording complete");
			}*/

		
        
        
        
        
        
        //About window ===============================================================================================


		private void BackButton2MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			splash.Visibility = System.Windows.Visibility.Visible;
			Read.Visibility = System.Windows.Visibility.Hidden;
			About.Visibility = System.Windows.Visibility.Hidden;
			settings.Visibility = System.Windows.Visibility.Hidden;

		}

		private void HelpButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(@"Resources\\Help.pdf");
			}
			catch (Exception)
			{
				MessageBox.Show("File does not found !", "Automatic Reader", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
		}


        
        
        //Settings window =================================================================================================

		private void SettingsBackButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			splash.Visibility = System.Windows.Visibility.Visible;
			Read.Visibility = System.Windows.Visibility.Hidden;
			About.Visibility = System.Windows.Visibility.Hidden;
			settings.Visibility = System.Windows.Visibility.Hidden;


			Properties.Settings.Default.defaultVolume = (int)_voice.Volume;
			Properties.Settings.Default.defaultRate = (int)_voice.Rate;
			Properties.Settings.Default.readerVoice = VoiceChange_ComboBox.SelectedIndex;
			Properties.Settings.Default.Save();

		}
	}
}