using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DynamicCreationAndCollision
{
	public partial class Bola : UserControl
	{
		private TextBox painelTop;
		private TextBox painelLeft;
		private double rootHeight;
		public double Velocidade;
		private bool monitorando;
		public bool Monitorando
		{
			get 
			{
				return monitorando;
			}
			set 
			{
				monitorando = value;
				if (value)
				{
					bola.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
				}
				else
				{
					bola.Stroke = null;
				}				
				if(MonitorandoChanged != null)
					MonitorandoChanged(this, new MonitorandoArgs(value));
			}
		}
		public double Esquerda 	{ get { return Canvas.GetLeft(this); } }
		public double Direita 	{ get { return Canvas.GetLeft(this) + this.Width ; } }
		public double Topo 		{ get { return Canvas.GetTop(this); } }
		public double Fundo 	{ get { return Canvas.GetTop(this) + this.Height ; } }
		
		public event MonitorandoHaldler MonitorandoChanged;
		
		public Bola(double Height, ref TextBox txtTop, ref TextBox txtLeft)
		{
			this.InitializeComponent();
			
			this.rootHeight = Height;
			this.painelTop = txtTop;
			this.painelLeft = txtLeft;
			this.Velocidade = (new Random(DateTime.Now.Millisecond).NextDouble()) * 2.0;
			this.monitorando = false;
         	
			Move.Completed += new EventHandler(Move_Completed);
            Move.Begin();
        }

        private void Move_Completed(object sender, EventArgs e)
        {
            Canvas.SetTop(this, Topo + Velocidade);

			if (this.Monitorando)
			{
				painelTop.Text = Topo.ToString("0.00");
				painelLeft.Text = Esquerda.ToString("0.00");
			}
			
            if (Topo + this.Height >= this.rootHeight)
            {
                Velocidade *= -1;
            }
            else if (Topo <= 0)
            {
                Velocidade *= -1;
            }
            Move.Begin();
        }

		private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.Monitorando = !this.Monitorando;
		}
		
		public void Terminar()
		{
			if (Move != null)
				Move.Stop();
			Canvas c = (Canvas)this.Parent;
			if (c != null)
				c.Children.Remove(this);
		}
	}
}