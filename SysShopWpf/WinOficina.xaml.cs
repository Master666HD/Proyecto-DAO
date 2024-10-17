using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using MySql.Data.MySqlClient;
using SysShopWpf;
using SysShopDAO.Implementation;
using SysShopDAO.Model;
namespace SysShopWpf
{
    /// <summary>
    /// Lógica de interacción para WinOficina.xaml
    /// </summary>
    public partial class WinOficina : Window
    {
        /// <summary>
        /// 1 Insert 2 Update
        /// </summary>
        byte op = 0;
        UsuarioImpl usuario;
        Usuario t;
        public WinOficina()
        {
            InitializeComponent();
        }

    void Select()

        {
            try
            {
                usuario = new UsuarioImpl();
                dgDatos.ItemsSource = null;
                dgDatos.ItemsSource = usuario.Select().DefaultView;
                dgDatos.Columns[0].Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void btnCerrar_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Select();
            
        }


        void Habilitar()
        {
            btnInsertar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = false;


            btnGuardar.IsEnabled = true;
            btnCanselar.IsEnabled = true;

            txtNombre.IsEnabled = true;
            txtDirec.IsEnabled = true;

            txtNombre.Focus();
        }
        void Deshabilitar()
        {
            btnInsertar.IsEnabled = true;
            btnModificar.IsEnabled = true;
            btnEliminar.IsEnabled = true;


            btnGuardar.IsEnabled = false;
            btnCanselar.IsEnabled = false;

            txtNombre.IsEnabled = false;
            txtDirec.IsEnabled = false;

            txtNombre.Text = "";
            txtDirec.Text = "";
        }

        private void btnInsertar_Click(object sender, RoutedEventArgs e)
        {
            Habilitar();
            this.op = 1;
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            Habilitar();
            this.op = 2;
        }

        private void btnCanselar_Click(object sender, RoutedEventArgs e)
        {
            Deshabilitar();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            switch (this.op)
            {
                case 1:
                    try
                    {
                        // Creación de un nuevo objeto Usuario con todos los campos
                        t = new Usuario(
                            txtCi.Text,
                            txtNombres.Text,
                            txtPrimerApellido.Text,
                            txtSegundoApellido.Text,
                            DateTime.Parse(txtFechaNacimiento.Text),
                            char.Parse(txtSexo.Text),
                            txtRol.Text,
                            nombreUsuario,  // Nombre de usuario generado automáticamente
                            contraseniaInicial,  
                            1 // Asignación de un ID de usuario
                        );

                        usuario = new UsuarioImpl();

                        if (usuario.Insert(t) > 0)
                        {
                            MessageBox.Show("Registro insertado con éxito");
                            Select(); // Actualiza la vista después de la inserción
                            Deshabilitar();
                        }
                        else
                        {
                            MessageBox.Show("No se insertaron registros");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;

                case 2:
                    if (t != null)
                    {
                        t.Ci = txtCi.Text;
                        t.Nombres = txtNombres.Text;
                        t.PrimerApellido = txtPrimerApellido.Text;
                        t.SegundoApellido = txtSegundoApellido.Text;
                        t.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                        t.Sexo = char.Parse(txtSexo.Text);
                        t.Rol = txtRol.Text;
                        

                        try
                        {
                            usuario = new UsuarioImpl();
                            if (usuario.Update(t) > 0)
                            {
                                MessageBox.Show("Registro modificado con éxito");
                                Select();
                                Deshabilitar();
                            }
                            else
                            {
                                MessageBox.Show("No se modificaron registros");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    break;
            }
        }

        private void dgDatos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dgDatos.SelectedItem != null && dgDatos.Items.Count > 0)
            {
                DataRowView d = (DataRowView)dgDatos.SelectedItem;
                short id = short.Parse(d.Row.ItemArray[0].ToString());  // Obtener el ID del usuario

                try
                {
                    usuario = new UsuarioImpl();
                    t = usuario.Get(id);

                    if (t != null)
                    {
                        txtCi.Text = t.Ci;
                        txtNombres.Text = t.Nombres;
                        txtPrimerApellido.Text = t.PrimerApellido;
                        txtSegundoApellido.Text = t.SegundoApellido;
                        txtFechaNacimiento.Text = t.FechaNacimiento.ToString("yyyy-MM-dd");
                        txtSexo.Text = t.Sexo.ToString();
                        txtRol.Text = t.Rol;
                        txtNombreUsuario.Text = t.NombreUsuario;
                        txtContrasenia.Text = t.Contrasenia;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (t != null)
            {
                if (MessageBox.Show("¿Está seguro de eliminar el registro?", "Eliminar",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        usuario = new UsuarioImpl();
                        if (usuario.Delete(t) > 0)
                        {
                            MessageBox.Show("Registro eliminado");
                            Select();
                        }
                        else
                        {
                            MessageBox.Show("No se eliminaron registros");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }
        }

       



    }
}
