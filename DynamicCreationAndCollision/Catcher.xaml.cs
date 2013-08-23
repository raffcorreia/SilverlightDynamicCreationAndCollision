using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace DynamicCreationAndCollision
{
	public partial class Pegador : UserControl
	{
		private Dictionary<int, Bola> bolas;
		private TextBox painelTop;
		private TextBox painelLeft;
		private double rootWidth;
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
					Retangulo.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
				}
				else
				{
					Retangulo.Stroke = null;
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
		public event PegouHaldler Pegou;
		
		public Pegador(double Width, ref TextBox txtTop, ref TextBox txtLeft, ref Dictionary<int, Bola> bolasDic)
		{
			// Required to initialize variables
			InitializeComponent();
			
			this.rootWidth = Width;
			this.painelTop = txtTop;
			this.painelLeft = txtLeft;
			this.Velocidade = (((new Random(DateTime.Now.Millisecond).NextDouble()) * 4.0) -2);
			this.monitorando = false;
			this.bolas = bolasDic;
         	
			Move.Completed += new EventHandler(Move_Completed);
            Move.Begin();
		}
		
		private void Pega()
		{
			foreach(KeyValuePair<int, Bola> k in bolas)
			{
				//Testa se esta na mesma direção horizontal
				if(k.Value.Esquerda >= this.Esquerda && k.Value.Esquerda <= this.Direita 
					|| k.Value.Direita >= this.Esquerda && k.Value.Direita <= this.Direita )
				{
					//Agora testa se está na mesma posição vertical
					if(k.Value.Topo >= this.Topo && k.Value.Topo <= this.Fundo 
						|| k.Value.Fundo >= this.Topo && k.Value.Fundo <= this.Fundo )
					{
						bolas.Remove(k.Key);
						k.Value.Terminar();
						if(Pegou != null)
							Pegou(this, null);
						//this.Terminar();
						return;
					}
				}
			}
		}
		
        private void Move_Completed(object sender, EventArgs e)
        {
            Canvas.SetLeft(this, Esquerda + Velocidade);
			Pega();
			if (this.monitorando)
			{
				painelTop.Text = Topo.ToString("0.00");
				painelLeft.Text = Esquerda.ToString("0.00");
			}
			
            if (Esquerda + this.Width >= this.rootWidth)
            {
                Velocidade *= -1;
            }
            else if (Esquerda <= 0)
            {
                Velocidade *= -1;
            }
            Move.Begin();
        }

		private void Retangulo_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
			this.bolas = null;
		}
	}
	
	public delegate void PegouHaldler(object sender, EventArgs e);
	
}