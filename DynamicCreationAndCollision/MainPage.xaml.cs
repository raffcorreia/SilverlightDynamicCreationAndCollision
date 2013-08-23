using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DynamicCreationAndCollision
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();
			
			txtBallTop.Text = "";
			txtBallLeft.Text = "";
			txtCatcherTop.Text = "";
			txtCatcherLeft.Text = "";
			selectedBall = 0;
			selectedCatcher = 0;
		}

		private Dictionary<int, Ball> balls = new Dictionary<int, Ball>();
		private void btnCreateBall_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Ball  myBall = new Ball(LayoutRoot.Height, ref txtBallTop, ref txtBallLeft);
			myBall.MonitoringChanged += new MonitoringHaldler(Ball_MonitoringChanged);
			Canvas.SetLeft(myBall, (new Random(DateTime.Now.Millisecond).NextDouble()) * (LayoutRoot.Width - myBall.Width));
            Canvas.SetTop(myBall, 0);
            LayoutRoot.Children.Add(myBall);
			balls.Add(myBall.GetHashCode(), myBall);
			selectBall();
			updateLabels();
		}

		private Dictionary<int, Catcher> catchers = new Dictionary<int, Catcher>();
		private void btnCreateCatcher_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Catcher theCatcher = new Catcher(LayoutRoot.Width, ref txtCatcherTop, ref txtCatcherLeft, ref balls);
			theCatcher.MonitoringChanged += new MonitoringHaldler(Catcher_MonitoringChanged);
			theCatcher.Caught += new CaughtHaldler(Catcher_Caught);
            Canvas.SetLeft(theCatcher, LayoutRoot.Width / 2);
            Canvas.SetTop(theCatcher, (new Random(DateTime.Now.Millisecond).NextDouble()) * (LayoutRoot.Height - theCatcher.Height));
            LayoutRoot.Children.Add(theCatcher);
			catchers.Add(theCatcher.GetHashCode(), theCatcher);
			selectCatcher();
			updateLabels();
		}
		
		private void btnBallDown_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			selectedBall--;
			if(selectedBall < 0)
				selectedBall = balls.Count - 1;
			selectBall();
		}

		private void btnBallUp_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			selectedBall++;
			if(selectedBall >= balls.Count)
				selectedBall = 0;
			selectBall();
		}
		
		Ball monitoredBall;
		private void Ball_MonitoringChanged(object sender, MonitoringArgs e)
		{
			if(e.valor) //Importante para evitar recursividade
			{
				if(monitoredBall != null) 
				{
					if(monitoredBall != (Ball)sender)
						monitoredBall.Monitoring = false;
				}			
				monitoredBall = (Ball)sender;
			}
		}

		private void Catcher_Caught(object sender, EventArgs e)
		{
			SBCaught.Begin();
			selectCatcher();
			selectBall();
			updateLabels();
		}
		
		int selectedBall;
		private void selectBall()
		{
			if(selectedBall >= 0 && selectedBall < balls.Count)
			{
				KeyValuePair<int, Ball> k = balls.ElementAt(selectedBall);
				Ball b =  k.Value;
				b.Monitoring = true;
				updateLabels();
			}
			else
			{
				selectedBall = 0;
			}
		}

		private void btnCatcherDown_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			selectedCatcher--;
			if(selectedCatcher < 0)
				selectedCatcher = catchers.Count - 1;
			selectCatcher();
		}

		private void btnCatcherUp_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			selectedCatcher++;
			if(selectedCatcher >= catchers.Count)
				selectedCatcher = 0;
			selectCatcher();
		}
		
		Catcher monitoredCatcher;
		private void Catcher_MonitoringChanged(object sender, MonitoringArgs e)
		{
			if(e.valor) //Importante para evitar recursividade
			{
				if(monitoredCatcher != null) 
				{
					if(monitoredCatcher != (Catcher)sender)
						monitoredCatcher.Monitoring = false;
				}			
				monitoredCatcher = (Catcher)sender;
			}
		}
		
		int selectedCatcher;
		private void selectCatcher()
		{
			if(selectedCatcher >= 0 && selectedCatcher < catchers.Count)
			{
				KeyValuePair<int, Catcher> k = catchers.ElementAt(selectedCatcher);
				Catcher p =  k.Value;
				p.Monitoring = true;
				updateLabels();
			}
			else
			{
				selectedCatcher = 0;
			}
		}

		private void updateLabels()
		{
			int b = selectedBall + 1;
			int p = selectedCatcher + 1;
			txtBall.Text = "Ball " + b.ToString() + " of " + balls.Count().ToString();
            txtCatcher.Text = "Catcher " + p.ToString() + " of " + catchers.Count().ToString();
		}

		private void btnDestroyCatcher_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (catchers.Count > 0)
			{
				if(selectedCatcher == catchers.Count - 1)
					selectedCatcher = -1;
				KeyValuePair<int, Catcher> k = catchers.ElementAt(catchers.Count - 1);
				k.Value.Finish();
				catchers.Remove(k.Key);
				updateLabels();
			}
		}

		private void LayoutRoot_Splash_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			LayoutRoot_Splash.Visibility = Visibility.Collapsed;
		}
	}
}