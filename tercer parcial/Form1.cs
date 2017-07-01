using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace tercer_parcial
{
    public partial class Form1 : Form
    {
        public int num_usuarios, num_registros;
        public struct Usuarios
        { 
            public int cuenta;
            public string nombre;
            public string contraseña;
            public string imagen;
        }
        public Usuarios[] usuarios;
        public struct Registros
        {
            public int cuenta;
            public string nombre;
            public DateTime entrada;
            public DateTime salida;
        }
        public Registros[] registros = new Registros[10000];
        public string[] temp = new string[4];
        public int N;

        public Form1()
        {
            InitializeComponent();

            ///Lee y ordena los datos de los usuarios
            FileStream stream = new FileStream("Usuarios.txt", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            for (int i = 0; reader.Peek() > -1; i++)
            {
                reader.ReadLine();
                num_usuarios++;
            }

            num_usuarios = num_usuarios - 1;
            usuarios = new Usuarios[num_usuarios];
            stream.Seek(0, SeekOrigin.Begin);
            N = Convert.ToInt32(reader.ReadLine());
            for (int i = 0; reader.Peek() > -1; i++)
            {
                string Linea = reader.ReadLine();
                temp = Linea.Split('-');

                if (temp[0] == null)
                {
                    break;
                }

                usuarios[i].cuenta = Convert.ToInt32(temp[0]);
                usuarios[i].nombre = temp[1];
                usuarios[i].contraseña = temp[2];
                usuarios[i].imagen = temp[3];
            }

            stream.Close();
            reader.Close();
        }

        public DateTime origen = new DateTime(0001, 01, 01, 00, 00, 00);
        private void textBox2_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                for (int i = 0; i < num_usuarios; i++)
                {
                    if (Convert.ToInt32(textBox1.Text) == usuarios[i].cuenta && textBox2.Text == usuarios[i].contraseña)  //revisa que el usuario y la contraseña sean correctos
                    {
                        for (int j = 0; j <= num_registros; j++)
                        {
                            if (registros[j].cuenta != 0)  //si el registro esta vacio, escribe uno nuevo
                            {
                                if (registros[j].cuenta == usuarios[i].cuenta && 0 == DateTime.Compare(registros[j].salida, origen))  //si el usuario YA esta registrado pero NO HA SALIDO
                                {
                                    registros[j].salida = DateTime.Now;

                                    pictureBox2.Image = Image.FromFile("img/" + usuarios[i].imagen + ".jpg");
                                    lNombre.Text = usuarios[i].nombre;
                                    lMensaje.Text = "Adios";
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                registros[num_registros].cuenta = usuarios[i].cuenta;
                                registros[num_registros].nombre = usuarios[i].nombre;
                                registros[num_registros].entrada = DateTime.Now;
                                num_registros++;

                                pictureBox2.Image = Image.FromFile("img/" + usuarios[i].imagen + ".jpg");
                                lNombre.Text = usuarios[i].nombre;
                                lMensaje.Text = "Bienvenido";
                                break;
                            }
                        }
                        textBox1.Clear();
                        textBox2.Clear();
                        label1.Visible = false;
                        label2.Visible = false;
                        textBox1.Visible = false;
                        textBox2.Visible = false;
                        pictureBox2.Visible = true;
                        lMensaje.Visible = true;
                        lNombre.Visible = true;
                        timer1.Interval = 2500;
                        timer1.Enabled = true;
                        return;
                    }
                }
                MessageBox.Show("Número de cuenta o contraseña incorrectos.");
                textBox1.Clear();
                textBox2.Clear();
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
            lMensaje.Visible = false;
            lNombre.Visible = false;
            label1.Visible = true;
            label2.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            //textBox1.Focus();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            FileStream stream = new FileStream("Registro" + (N + 1) + ".txt", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            for (int i = 0, x = 0; i < 10000 && x < num_registros; i++)
            {
                if (registros[i].cuenta != 0)
                {
                    string linea = registros[i].cuenta + "-" + registros[i].nombre + "-" + registros[i].entrada + "-" + registros[i].salida;
                    writer.WriteLine(linea);
                    x++;
                }
            }
            writer.Close();
            stream.Close();


            /*
            *   Actualiza es documento USUARIOS para contar el numero de registros "N" realizados
            */
            N++;
            stream = new FileStream("Usuarios.txt", FileMode.Create, FileAccess.Write);
            writer = new StreamWriter(stream);
            writer.WriteLine(N);
            for (int i = 0; i < num_usuarios; i++)
            {
                if (usuarios[i].cuenta != 0)
                {
                    string linea = usuarios[i].cuenta + "-" + usuarios[i].nombre + "-" + usuarios[i].contraseña + "-" + usuarios[i].imagen;
                    writer.WriteLine(linea);
                }
            }
            writer.Close();
            stream.Close();
        }
    }
}