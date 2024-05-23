using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prueba_SignalR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        HubConnection hub;
        private async void Form1_Load(object sender, EventArgs e)
        {
            hub = new HubConnectionBuilder().WithUrl("https://octavo.itesrc.net/nombres").WithAutomaticReconnect().Build();

            hub.On<string>("NombreNuevo", x=>
            {
                this.BeginInvoke(() =>
                {
                    lstNombres.Items.Add(x);
                    lstNombres.SelectedIndex = lstNombres.Items.Count - 1;
                });
                
            });

            hub.On<string>("NombreBorrado", x =>
            {
                this.BeginInvoke(() =>
                {
                    lstNombres.Items.Add(x);
                });

            });

            await hub.StartAsync();

        }

        private void BeginInvoke(Action value)
        {
            throw new NotImplementedException();
        }

        private async void btnAgregar_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(btnAgregar.Text))
            {
                await hub.InvokeAsync("AgregarNombre", txtNombre.Text);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lstNombres.Text))
            {
                await hub.InvokeAsync("EliminarNombre", lstNombres.Text);
            }
        }
    }
}
