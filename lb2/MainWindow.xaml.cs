using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lb2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PerspectiveCamera cam;
        public MainWindow()
        {
            InitializeComponent();
            Title = "Simple 3D Scene in Code";

            // Make DockPanel content of window.
            DockPanel dock = new DockPanel();           
            Content = dock;

            // Create Scrollbar for moving camera
            ScrollBar scroll = new ScrollBar();
            scroll.Orientation = Orientation.Horizontal;
            scroll.Value = -0.2;
            scroll.Minimum = -0.5;
            scroll.Maximum = 0.5;
            scroll.ValueChanged += ScrollBarOnValueChanged;
            dock.Children.Add(scroll);
            DockPanel.SetDock(scroll, Dock.Bottom);

            // Create Viewport3D for 3D scene
            Viewport3D viewport = new Viewport3D();
            dock.Children.Add(viewport);

            // Define the GeometryModel3D
            GeometryModel3D geomod = new GeometryModel3D();

            geomod.Geometry = meshPyramid(1.2,1.5,5);

            geomod.Material = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(128, 0, 255, 255)));
            geomod.BackMaterial = new DiffuseMaterial(Brushes.Gray);

            // Create ModelVisual3D for GeometryModel3D
            ModelVisual3D modvis = new ModelVisual3D();
            modvis.Content = geomod;
            viewport.Children.Add(modvis);

            // Create another ModelVisual3D for light
            modvis = new ModelVisual3D();
            modvis.Content = new AmbientLight(Colors.White);
            viewport.Children.Add(modvis);

            // Create the camera
            cam = new PerspectiveCamera(new Point3D(-1, 0.5, 4),
                         new Vector3D(0, 0, -1), new Vector3D(0, 1.5, 0), 45);
            viewport.Camera = cam;
        }
        void ScrollBarOnValueChanged(object sender,
                           RoutedPropertyChangedEventArgs<double> args)
        {
            cam.Position = new Point3D(args.NewValue, cam.Position.Y, cam.Position.Z);
        }

        // Create mesh. Set the height of the shape, the width and the number of sides at the base of the shape
        public MeshGeometry3D meshPyramid(double hight, double width, int count)
        {
            double corner=(2*Math.PI) / count;
            corner=Math.Round(corner, 2);

            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(0, hight, 0));
            mesh.TriangleIndices = new Int32Collection(new int[] { 0 });

            for (int i = 0; i < count; i++)
            {
                double x = Math.Sin(corner * i)*width/2;
                double z = Math.Cos(corner * i)*width/2;
                
                mesh.Positions.Add(new Point3D(Math.Round(x, 2), 0, Math.Round(z, 2)));
                
                mesh.TriangleIndices.Add(i+1);
                if (i> 0) { 
                    mesh.TriangleIndices.Add(0);
                    mesh.TriangleIndices.Add(i+1);
                } if (i==count-1) mesh.TriangleIndices.Add(1);

            }
            return mesh;
        }
    }
}
