using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K_Means_Clustering_Algorithm
{
    public partial class Form1 : MetroForm
    {
        Graphics g;
        const int POINT_HEIGHT = 1;
        const int LIMIT = 100000;
        int n = 0;
        int[] X = new int[LIMIT];
        int[] Y = new int[LIMIT];
        public Form1()
        {
            InitializeComponent();
            g = panel.CreateGraphics();
        }

        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                g.DrawEllipse(Pens.Black, e.X, e.Y, POINT_HEIGHT, POINT_HEIGHT);
                X[n] = e.X;
                Y[n] = e.Y;
                n++;
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            panel.Refresh(); // Refresh Panel to Remove Previos Drawings
            for (int i = 0; i < n; i++) // Display Points on the Panel
                g.DrawEllipse(Pens.Black, X[i], Y[i], POINT_HEIGHT, POINT_HEIGHT);
            list.Items.Clear(); // Clear List

            int clusters = 0;
            try
            {
                clusters = int.Parse(txt_Clusters.Text);
            }
            catch 
            {
                MessageBox.Show("Invalid Inpu");
                return;
            }
            double[] centroids_X = new double[clusters];
            double[] centroids_Y = new double[clusters];
            double[] prev_centroids_X = new double[clusters];
            double[] prev_centroids_Y = new double[clusters];
            int[,] nearest_points = new int[clusters, LIMIT];
            int[] points_count = new int[clusters];

            if (n < clusters)
            {
                MessageBox.Show("Not Enough Points");
                return;
            }
            /* Mark Initial Centroids */
            Random R = new Random();
            for (int i = 0; i < clusters; i++)
            {
                centroids_X[i] = X[R.Next() % n];
                centroids_Y[i] = Y[R.Next() % n];
            }
            bool Ended = false;
            /* Infinite Loop - Until Centeroids Find */
            for (int i = 0; !Ended ; i++) // i => Iteration 
            {
                for (int j = 0; j < n; j++) // j => Related Point
                {
                    double min = int.MaxValue;
                    int nearest_cluster = 0;
                    for (int k = 0; k < clusters; k++) // K => Cluster
                    {
                        double Distance = GetDistance(X[j], Y[j], centroids_X[k], centroids_Y[k]);
                        if (Distance < min)
                        {
                            min = Distance;
                            nearest_cluster = k;
                        }
                    }
                    nearest_points[nearest_cluster, points_count[nearest_cluster]++] = j;
                }
                /* Finalizing Iteration */
                list.Items.Add("Iteration : " + (i + 1));
                for (int m = 0; m < clusters; m++) //m => cluster
                {
                    double sumX = 0;
                    double sumY = 0;
                    for (int n = 0; n < points_count[m]; n++) //n => Nearest Points to the Cluster {m}
                    {
                        sumX += X[nearest_points[m, n]];
                        sumY += Y[nearest_points[m, n]];
                    }
                    if(points_count[m] == 0)
                    {
                        MessageBox.Show("Randomly Selected Points do not match with the Data Set\n" +
                            "Please Restart the Process");
                        return;
                    }
                    centroids_X[m] = sumX / points_count[m];
                    centroids_Y[m] = sumY / points_count[m];
                    list.Items.Add(String.Format("C{0} : [ X => {1:000.00} ]  [ Y => {2:000.00} ]", m + 1,
                        centroids_X[m], centroids_Y[m]));
                    points_count[m] = 0;
                }
                /* Check Whether Were the  Positions of Centroids changed  
                   related to the previous step*/
                bool Flag = true;
                for (int m = 0; m < clusters; m++) //m => cluster
                {
                    if (!Flag || centroids_X[m] != prev_centroids_X[m])
                        Flag = false;
                    if (!Flag || centroids_X[m] != prev_centroids_X[m])
                        Flag = false;
                    prev_centroids_X[m] = centroids_X[m];
                    prev_centroids_Y[m] = centroids_Y[m];
                }
                Ended = Flag;
            }
            for (int m = 0; m < clusters; m++) //m => cluster
            {
                g.FillEllipse(Brushes.Red, (float)centroids_X[m], (float)centroids_Y[m], POINT_HEIGHT * 5, POINT_HEIGHT * 5);
            }
        }
        private static double GetDistance(double x1,double y1,double x2,double y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            panel.Refresh();
            n = 0;
        }
    }
}
