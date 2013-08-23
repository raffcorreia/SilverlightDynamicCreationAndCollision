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
			
			txtBolaTop.Text = "";
			txtBolaLeft.Text = "";
			txtPegadorTop.Text = "";
			txtPegadorLeft.Text = "";
			bolaSelecionada = 0;
			pegadorSelecionado = 0;
		}

		private Dictionary<int, Bola> bolas = new Dictionary<int, Bola>();
		private void btnCriaBola_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Bola  minhaBola = new Bola(LayoutRoot.Height, ref txtBolaTop, ref txtBolaLeft);
			minhaBola.MonitorandoChanged += new MonitorandoHaldler(Bola_MonitorandoChanged);
			Canvas.SetLeft(minhaBola, (new Random(DateTime.Now.Millisecond).NextDouble()) * (LayoutRoot.Width - minhaBola.Width));
            Canvas.SetTop(minhaBola, 0);
            LayoutRoot.Children.Add(minhaBola);
			bolas.Add(minhaBola.GetHashCode(), minhaBola);
			selecionaBola();
			atualizaLabels();
		}

		private Dictionary<int, Pegador> pegadores = new Dictionary<int, Pegador>();
		private void btnCriaPegador_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Pegador oPegador = new Pegador(LayoutRoot.Width, ref txtPegadorTop, ref txtPegadorLeft, ref bolas);
			oPegador.MonitorandoChanged += new MonitorandoHaldler(Pegador_MonitorandoChanged);
			oPegador.Pegou += new PegouHaldler(Pegador_Pegou);
            Canvas.SetLeft(oPegador, LayoutRoot.Width / 2);
            Canvas.SetTop(oPegador, (new Random(DateTime.Now.Millisecond).NextDouble()) * (LayoutRoot.Height - oPegador.Height));
            LayoutRoot.Children.Add(oPegador);
			pegadores.Add(oPegador.GetHashCode(), oPegador);
			selecionaPegador();
			atualizaLabels();
		}
		
		private void btnBolaDown_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			bolaSelecionada--;
			if(bolaSelecionada < 0)
				bolaSelecionada = bolas.Count - 1;
			selecionaBola();
		}

		private void btnBolaUp_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			bolaSelecionada++;
			if(bolaSelecionada >= bolas.Count)
				bolaSelecionada = 0;
			selecionaBola();
		}
		
		Bola bolaMonitorada;
		private void Bola_MonitorandoChanged(object sender, MonitorandoArgs e)
		{
			if(e.valor) //Importante para evitar recursividade
			{
				if(bolaMonitorada != null) 
				{
					if(bolaMonitorada != (Bola)sender)
						bolaMonitorada.Monitorando = false;
				}			
				bolaMonitorada = (Bola)sender;
			}
		}

		private void Pegador_Pegou(object sender, EventArgs e)
		{
			SBPegou.Begin();
			selecionaPegador();
			selecionaBola();
			atualizaLabels();
		}
		
		int bolaSelecionada;
		private void selecionaBola()
		{
			if(bolaSelecionada >= 0 && bolaSelecionada < bolas.Count)
			{
				KeyValuePair<int, Bola> k = bolas.ElementAt(bolaSelecionada);
				Bola b =  k.Value;
				b.Monitorando = true;
				atualizaLabels();
			}
			else
			{
				bolaSelecionada = 0;
			}
		}

		private void btnPegadorDown_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pegadorSelecionado--;
			if(pegadorSelecionado < 0)
				pegadorSelecionado = pegadores.Count - 1;
			selecionaPegador();
		}

		private void btnPegadorUp_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			pegadorSelecionado++;
			if(pegadorSelecionado >= pegadores.Count)
				pegadorSelecionado = 0;
			selecionaPegador();
		}
		
		Pegador pegadorMonitorado;
		private void Pegador_MonitorandoChanged(object sender, MonitorandoArgs e)
		{
			if(e.valor) //Importante para evitar recursividade
			{
				if(pegadorMonitorado != null) 
				{
					if(pegadorMonitorado != (Pegador)sender)
						pegadorMonitorado.Monitorando = false;
				}			
				pegadorMonitorado = (Pegador)sender;
			}
		}
		
		int pegadorSelecionado;
		private void selecionaPegador()
		{
			if(pegadorSelecionado >= 0 && pegadorSelecionado < pegadores.Count)
			{
				KeyValuePair<int, Pegador> k = pegadores.ElementAt(pegadorSelecionado);
				Pegador p =  k.Value;
				p.Monitorando = true;
				atualizaLabels();
			}
			else
			{
				pegadorSelecionado = 0;
			}
		}

		private void atualizaLabels()
		{
			int b = bolaSelecionada + 1;
			int p = pegadorSelecionado + 1;
			txtBola.Text = "Bola " + b.ToString() + " de " + bolas.Count().ToString();
			txtPegador.Text = "Pegador " + p.ToString() + " de " + pegadores.Count().ToString();
		}

		private void btnDestroiPegador_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (pegadores.Count > 0)
			{
				if(pegadorSelecionado == pegadores.Count - 1)
					pegadorSelecionado = -1;
				KeyValuePair<int, Pegador> k = pegadores.ElementAt(pegadores.Count - 1);
				k.Value.Terminar();
				pegadores.Remove(k.Key);
				atualizaLabels();
			}
		}

		private void LayoutRoot_Splash_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			LayoutRoot_Splash.Visibility = Visibility.Collapsed;
		}
	}
}