# K-Means++ McCaffrey Implementation Visualized On MNIST

## K-Means++ With K = 8

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/main_kmeans.png?raw=true">
</p>

Instead of using a synthetic dataset as used in the source demo, I simply adapted the input to the MNIST dataset and was able to use an extremely sophisticated K-Means++ algorithm for free. Cool!

What you see here is a clustering of 60,000 MNIST training samples into 8 clusters. The cluster count shows the distribution of all samples into the different clusters. The cluster mean shows the image that visually describes the mean of the respective cluster. The cluster samples are in this case the first 100 training data distributed over 8 clusters. 

[K-Means++ Implementation](https://visualstudiomagazine.com/Articles/2020/05/06/data-clustering-k-means.aspx?Page=1)

[Why this implementation?](https://www.youtube.com/watch?v=6oUW9IYbhEc)

[Further use](https://www.youtube.com/watch?v=yR7k19YBqiw)





