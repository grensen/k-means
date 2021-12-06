using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

public class TheWindow : Window
{

    string initMethod = "plusplus";
    int seed    = 0;
    int imgSize = 1000; // number samples  
    int K       = 5; // number clusters    
    int maxIter = 10; // max (likely less)
    int trials  = 10; // attempts to find best

    readonly string path = @"C:\goodgame\one\";
    readonly SolidColorBrush brFont = new(Color.FromRgb(205, 199, 168));
    readonly SolidColorBrush brFont2 = new(Color.FromRgb(9, 6, 0));
    Canvas canGlobal = new();
    float[] samples;
    int[] labels;

    int mnistX = 30;
    int maxSamples = 200;
    [STAThread]
    public static void Main() { new Application().Run(new TheWindow()); }

    // CONSTRUCTOR - LOADED - ONINIT
    private TheWindow() // constructor
    {
        Title = "McCaffrey K-Means++ On MNIST"; //        
        Content = canGlobal;
        Background = Brushes.Black; // new SolidColorBrush(Color.FromRgb(50, 50, 50));
        Width = K * 160 + 50; // 24 + 10 + 5 + 5 + 30 + 28*9 + 600 + 100 + 30; // WidthG;
        Height = 700; // HeightG;                   

        LoadMnistAndConvert();
                           
        DrawingContext dc = ContextHelpMod(false, ref canGlobal);
        Run(ref dc);
        dc.Close();
        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate { }));
    } // TheWindow end

    void Run(ref DrawingContext dc)
    {
        // support 
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo("en-us");
        Typeface tf = new("TimesNewRoman"); // "Arial"

        // convert data to 2D array
        double[][] data = new double[imgSize][];
        for (int i = 0, c = 0; i < imgSize; i++)
        {
            data[i] = new double[784];
            for (int j = 0; j < 784; j++)
                data[i][j] = samples[c++];
        }

        // run K-Means++
        my_kmeans(ref dc);

        void my_kmeans(ref DrawingContext dc)
        {
            Brush[] br = new Brush[10];
            br[1] = BrF(128, 192, 0); // lime green
            br[0] = BrF(255, 0, 0); // red
            br[2] = BrF(64, 0, 255); // blue
            br[3] = BrF(161, 195, 255); // baby blue                  
            br[4] = BrF(0, 255, 0); // green
            br[5] = BrF(255, 0, 255); // magenta
            br[6] = BrF(75, 0, 130); // indigo
            br[7] = BrF(0, 128, 128); // teal
            br[8] = BrF(128, 128, 0); // olive
            br[9] = BrF(255, 255, 0); // yellow

            dc.DrawText(new FormattedText("McCaffrey K-Means++ on MNIST", ci, FlowDirection.LeftToRight, tf, 12, brFont, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                new Point(mnistX + 1, 20));
            dc.DrawText(new FormattedText("K = " + K.ToString(), ci, FlowDirection.LeftToRight, tf, 12, brFont, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                new Point(mnistX + 1, 35));
            dc.DrawText(new FormattedText("Samples = " + imgSize.ToString(), ci, FlowDirection.LeftToRight, tf, 12, brFont, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                new Point(mnistX + 1, 50));
            dc.DrawText(new FormattedText("Iterations = " + maxIter.ToString(), ci, FlowDirection.LeftToRight, tf, 12, brFont, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                new Point(mnistX + 1, 65));
            dc.DrawText(new FormattedText("Trials = " + trials.ToString(), ci, FlowDirection.LeftToRight, tf, 12, brFont, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                new Point(mnistX + 1, 80));

            dc.DrawText(new FormattedText("Cluster count:", ci, FlowDirection.LeftToRight, tf, 12, brFont, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                new Point(mnistX + 1, 110));
            dc.DrawText(new FormattedText("Cluster mean:", ci, FlowDirection.LeftToRight, tf, 12, brFont, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                new Point(mnistX + 1, 157));
            dc.DrawText(new FormattedText("Cluster samples:", ci, FlowDirection.LeftToRight, tf, 12, brFont, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                new Point(mnistX + 1, 274));

            KMeans km = new KMeans(K, data, initMethod, maxIter, seed);
            km.Cluster(trials);

            // draw cluster count
            for (int i = 0; i < K; i++)
            {
                dc.DrawRectangle(br[i], null, new Rect(mnistX + i * 160, 131, 45, 20));
                dc.DrawText(new FormattedText(km.counts[i].ToString(), ci, FlowDirection.LeftToRight, tf, 12, brFont2, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                    new Point(mnistX + i * 160 + 5, 135));
            }

            // draw cluster mean
            for (int p = 0; p < K; p++)
                for (int i = 0, c = 0; i < 28; i++) for (int j = 0; j < 28; j++, c++)
                    {
                        float nj = (float)km.means[p][c];
                        if (nj > 0.35) // cut the lows for peformence
                            dc.DrawRectangle(BrF(255 * nj, 180 * nj, 0), null,
                                new Rect(mnistX + 5 * j + p * 160, 5 * i + 150, 4, 4));
                    }

            int[] classCount = new int[100];
            for (int pid = 0; pid < maxSamples; pid++)
            {
                int nid = km.clustering[pid];
                int nid2 = classCount[nid];
                if (nid2 < 60)
                    for (int i = 0, c = 0; i < 28; i++)
                        for (int j = 0; j < 28; j++, c++)
                        {
                            float nj = (float)km.data[pid][c];
                            if (nj > 0.35) // cut the lows for peformence
                                dc.DrawRectangle(BrF(255 * nj, 180 * nj, 0),
                                    null, new Rect(mnistX + 1 * j + (nid * 160 + (nid2 % 5) * 28), 1 * i + 190 + 100 + (nid2 / 5) * 28, 1, 1));
                        }
                classCount[nid]++;
            }

        }

    }


    void LoadMnistAndConvert()
    {
        Array.Resize<float>(ref samples, (60000 + 10000) * 784);
        Array.Resize<int>(ref labels, 60000 + 10000);

        for (int i = 0, c = 0, c2 = 0, dlen = 60000; i < 2; i++, dlen = 10000)
        {
            // load file
            FileStream image = new(i != 0 ? path + @"MNIST_Data\t10k-images.idx3-ubyte" : path + @"MNIST_Data\train-images.idx3-ubyte", FileMode.Open);
            FileStream label = new(i != 0 ? path + @"MNIST_Data\t10k-labels.idx1-ubyte" : path + @"MNIST_Data\train-labels.idx1-ubyte", FileMode.Open);

            // get start data
            image.Seek(16 + (i != 0 ? (1 - 1) : 0) * 784, 0);
            label.Seek(8 + (i != 0 ? (1 - 1) : 0), 0);

            // transmit data
            for (int x = 0; x < dlen; x++)
            {
                for (int n = 0; n < 784; ++n)
                    samples[c++] = image.ReadByte() / 255f;
                labels[c2++] = label.ReadByte();
            }
            image.Close(); label.Close();
        }
    } // init MNIST dataset end
    public class KMeans
    {
        public int K;  // number clusters (use lower k for indexing)
        public double[][] data;  // data to be clustered
        public int N;  // number data items
        public int dim;  // number values in each data item
        public string initMethod;  // "plusplus", "forgy" "random"
        public int maxIter;  // max per single clustering attempt
        public int[] clustering;  // final cluster assignments
        public double[][] means;  // final cluster means aka centroids
        public double wcss;  // final total within-cluster sum of squares (inertia??)
        public int[] counts;  // final num items in each cluster
        public Random rnd;  // for initialization

        public KMeans(int K, double[][] data, string initMethod, int maxIter, int seed)
        {
            this.K = K;
            this.data = data;  // reference copy
            this.initMethod = initMethod;
            this.maxIter = maxIter;

            this.N = data.Length;
            this.dim = data[0].Length;

            this.means = new double[K][];  // one mean per cluster
            for (int k = 0; k < K; ++k)
                this.means[k] = new double[this.dim];
            this.clustering = new int[N];  // cell val is cluster ID, index is data item
            this.counts = new int[K];  // one cell per cluster
            this.wcss = double.MaxValue;  // smaller is better

            this.rnd = new Random(seed);
        } // ctor

        public void Cluster(int trials)
        {
            for (int trial = 0; trial < trials; ++trial)
                Cluster();  // find a clustering and update bests
        }

        public void Cluster()
        {
            int[] currClustering = new int[this.N];  // [0, 0, 0, 0, .. ]

            double[][] currMeans = new double[this.K][];
            for (int k = 0; k < this.K; ++k)
                currMeans[k] = new double[this.dim];

            if (this.initMethod == "plusplus")
                InitPlusPlus(this.data, currClustering, currMeans, this.rnd);
            else
                throw new Exception("not supported");

            bool changed;  //  result from UpdateClustering (to exit loop)
            int iter = 0;
            while (iter < this.maxIter)
            {
                UpdateMeans(currMeans, this.data, currClustering);
                changed = UpdateClustering(currClustering, this.data, currMeans);
                if (changed == false) break;  // need to stop iterating
                ++iter;
            }

            double currWCSS = ComputeWithinClusterSS(this.data,
              currMeans, currClustering);
            if (currWCSS < this.wcss)  // new best clustering found
            {
                // copy the clustering, means; compute counts; store WCSS
                for (int i = 0; i < this.N; ++i)
                    this.clustering[i] = currClustering[i];

                for (int k = 0; k < this.K; ++k)
                    for (int j = 0; j < this.dim; ++j)
                        this.means[k][j] = currMeans[k][j];

                this.counts = ComputeCounts(this.K, currClustering);
                this.wcss = currWCSS;
            }

        } // Cluster()

        private static void InitPlusPlus(double[][] data,
          int[] clustering, double[][] means, Random rnd)
        {
            //  k-means++ init using roulette wheel selection
            // clustering[] and means[][] exist
            int N = data.Length;
            int dim = data[0].Length;
            int K = means.Length;

            // select one data item index at random as 1st meaan
            int idx = rnd.Next(0, N); // [0, N)
            for (int j = 0; j < dim; ++j)
                means[0][j] = data[idx][j];

            for (int k = 1; k < K; ++k) // find each remaining mean
            {
                double[] dSquareds = new double[N]; // from each item to its closest mean
                for (int i = 0; i < N; ++i) // for each data item
                {
                    // compute distances from data[i] to each existing mean (to find closest)
                    double[] distances = new double[k]; // we currently have k means

                    for (int ki = 0; ki < k; ++ki)
                        distances[ki] = EucDistance(data[i], means[ki]);

                    int mi = ArgMin(distances);  // index of closest mean to curr item
                    dSquareds[i] = distances[mi] * distances[mi];  // sq dist from item to its closest mean
                } // i

                // select an item far from its mean using roulette wheel

                int newMeanIdx = ProporSelect(dSquareds, rnd);
                for (int j = 0; j < dim; ++j)
                    means[k][j] = data[newMeanIdx][j];
            } // k remaining means
            UpdateClustering(clustering, data, means);
        } // InitPlusPlus

        static int ProporSelect(double[] vals, Random rnd)
        {
            // roulette wheel selection
            // on the fly technique
            // vals[] can't be all 0.0s
            int n = vals.Length;

            double sum = 0.0;
            for (int i = 0; i < n; ++i)
                sum += vals[i];

            double cumP = 0.0;  // cumulative prob
            double p = rnd.NextDouble();

            for (int i = 0; i < n; ++i)
            {
                cumP += (vals[i] / sum);
                if (cumP > p) return i;
            }
            return n - 1;  // last index
        }

        private static int[] ComputeCounts(int K, int[] clustering)
        {
            int[] result = new int[K];
            for (int i = 0; i < clustering.Length; ++i)
            {
                int cid = clustering[i];
                ++result[cid];
            }
            return result;
        }

        private static void UpdateMeans(double[][] means,
          double[][] data, int[] clustering)
        {
            // compute the K means using data and clustering
            // assumes no empty clusters in clustering

            int K = means.Length;
            int N = data.Length;
            int dim = data[0].Length;

            int[] counts = ComputeCounts(K, clustering);  // needed for means

            for (int k = 0; k < K; ++k)  // make sure no empty clusters
                if (counts[k] == 0)
                    throw new Exception("empty cluster passed to UpdateMeans()");

            double[][] result = new double[K][];  // new means
            for (int k = 0; k < K; ++k)
                result[k] = new double[dim];

            for (int i = 0; i < N; ++i)  // each data item
            {
                int cid = clustering[i];  // which cluster ID?
                for (int j = 0; j < dim; ++j)
                    result[cid][j] += data[i][j];  // accumulate
            }

            // divide accum sums by counts to get means
            for (int k = 0; k < K; ++k)
                for (int j = 0; j < dim; ++j)
                    result[k][j] /= counts[k];

            // no 0-count clusters so update the means
            for (int k = 0; k < K; ++k)
                for (int j = 0; j < dim; ++j)
                    means[k][j] = result[k][j];
        }

        private static bool UpdateClustering(int[] clustering,
          double[][] data, double[][] means)
        {
            int K = means.Length;
            int N = data.Length;

            int[] result = new int[N];  // proposed new clustering (cluster assignments)
            bool change = false;  // is there a change to the existing clustering?
            int[] counts = new int[K];  // check if new clustering makes an empty cluster

            for (int i = 0; i < N; ++i)  // make of copy of existing clustering
                result[i] = clustering[i];

            for (int i = 0; i < data.Length; ++i)  // each data item
            {
                double[] dists = new double[K];  // dist from curr item to each mean
                for (int k = 0; k < K; ++k)
                    dists[k] = EucDistance(data[i], means[k]);

                int cid = ArgMin(dists);  // index of the smallest distance
                if ((result[i] = cid) != clustering[i])
                    change = true;  // the proposed clustering is different for at least one item
                ++counts[cid];
            }

            if (change == false) return false;  // no change to clustering -- clustering has converged

            for (int k = 0; k < K; ++k)
                if (counts[k] == 0) return false;  // no change to clustering because would have an empty cluster

            // there was a change and no empty clusters so update clustering
            for (int i = 0; i < N; ++i) clustering[i] = result[i];

            return true;  // successful change to clustering so keep looping
        }

        private static double EucDistance(double[] item, double[] mean)
        {
            double sum = 0.0;
            for (int j = 0; j < item.Length; ++j)
            {
                double d = item[j] - mean[j];
                sum += d * d;
            }
            return Math.Sqrt(sum);
        }

        private static int ArgMin(double[] v)
        {
            double minVal = v[0]; int minIdx = 0;
            for (int i = 0; i < v.Length; ++i)
                if (v[i] < minVal)
                { minVal = v[i]; minIdx = i; }
            return minIdx;
        }

        private static double ComputeWithinClusterSS(double[][] data,
          double[][] means, int[] clustering)
        {
            double sum = 0.0;
            for (int i = 0; i < data.Length; ++i)
            {
                int cid = clustering[i];  // which cluster does data[i] belong to?
                sum += SumSquared(data[i], means[cid]);
            }
            return sum;
        }

        private static double SumSquared(double[] item, double[] mean)
        {
            double sum = 0.0;
            for (int j = 0; j < item.Length; ++j)
            {
                double d = item[j] - mean[j];
                sum += d * d;
            }
            return sum;
        }
    } // class KMeans


    static DrawingContext ContextHelpMod(bool isInit, ref Canvas cTmp)
    {
        if (!isInit) cTmp.Children.Clear();
        DrawingVisualElement drawingVisual = new();
        cTmp.Children.Add(drawingVisual);
        return drawingVisual.drawingVisual.RenderOpen();
    }
    static Brush BrF(float red, float green, float blue)
    {
        Brush frozenBrush = new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
        frozenBrush.Freeze();
        return frozenBrush;
    }
} // TheWindow end
public class DrawingVisualElement : FrameworkElement
{
    private readonly VisualCollection _children;
    public DrawingVisual drawingVisual;
    public DrawingVisualElement()
    {
        _children = new VisualCollection(this);
        drawingVisual = new DrawingVisual();
        _children.Add(drawingVisual);
    }
    protected override int VisualChildrenCount
    {
        get { return _children.Count; }
    }
    protected override Visual GetVisualChild(int index)
    {
        if (index < 0 || index >= _children.Count)
            throw new();
        return _children[index];
    }
} // DrawingVisualElement


