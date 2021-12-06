# K-Means++ McCaffrey Implementation Visualized On MNIST

## K-Means++ With K = 10

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/main_kmeans.png?raw=true">
</p>

Instead of using a synthetic dataset as used in the source demo, I simply adapted the input to the MNIST dataset and was able to use an extremely sophisticated K-Means++ algorithm for free. Cool!

What you see here is a clustering of 60,000 MNIST training samples into 10 clusters. The cluster count shows the distribution of all samples into the different clusters. The cluster mean shows the image that visually describes the mean of the respective cluster. The cluster samples are in this case the first 100 training data distributed over 10 clusters. 

## Clustering with 100 Samples

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_demo_01.png?raw=true">
</p>

Now let's see what happens when we distribute only the first 100 examples to only 3 clusters and different settings. Here, the clustering becomes solid from 30 iterations and 30 trials. Raising both values to 100 did not bring any change.

## Clustering with 10000 Samples

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_demo_02.png?raw=true">
</p>

With more examples, the picture becomes increasingly solid and all test settings ultimately lead to the same result. This is a pretty good sign to start the big tests, hoping that with even more images and less aggressive settings, fast and good results can be expected.

## K = 2

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K2.png?raw=true">
</p>

## K = 3

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K3.png?raw=true">
</p>

## K = 4

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K4.png?raw=true">
</p>

## K = 5

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K5.png?raw=true">
</p>

## K = 6

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K6.png?raw=true">
</p>

## K = 7

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K7.png?raw=true">
</p>

## K = 8

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K8.png?raw=true">
</p>

## K = 9

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K9.png?raw=true">
</p>

## K = 10

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/k-means_mnist_K10.png?raw=true">
</p>

---

Why all this? My goal is to specialize different neural networks to make better predictions. Whether this will work is not entirely certain, but it looks good.

### [K-Means++ Implementation](https://visualstudiomagazine.com/Articles/2020/05/06/data-clustering-k-means.aspx?Page=1)

### [Why this implementation?](https://www.youtube.com/watch?v=6oUW9IYbhEc)

### [Further use](https://www.youtube.com/watch?v=yR7k19YBqiw)





