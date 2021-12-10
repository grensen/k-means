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

With more examples, the picture becomes increasingly solid and all test settings lead to almost the same result. This is a pretty good sign to start the big tests, hoping that with even more images and less aggressive settings, fast and good results can be expected.

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

Why all this? My ultimate goal is not cluster the samples, instead it seems a good idea to take the error cases of a well-trained neural network to cluster them. This way, specialized neural networks can be trained to make better predictions. Whether this will work is not entirely sure, but it looks promising.


<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/error_cases_example.png?raw=true">
</p>

And this is roughly what it looks like. The red cluster contains the predicted examples that are "reliable". The green and blue clusters contain the special cases where the general network is not sure. By clustering the problem case, the problem example is then passed to the specialized network which is trained for that cluster. 

<p align="center">
  <img src="https://github.com/grensen/k-means/blob/main/figures/errorCases_MaxErrorCasesK10_03?raw=true">
</p>

And this is how it looks when the examples become even more extreme and are distributed in even more clusters. Good as an extreme example, but probably bad because too few examples are spread over too many clusters.

But all this is still a lot of work, first the problem cases have to be trained by the specialized networks, then the respective cluster of the error cases has to be stored. After all this, the general network must be put in front of the system and everything must work together correctly. 

It's not yet clear to me exactly how such a system should be structured, as there are countless choices as to how the whole thing could be implemented. But then hopefully a correct prediction will be made. We will see...

### [K-Means++ Implementation](https://visualstudiomagazine.com/Articles/2020/05/06/data-clustering-k-means.aspx?Page=1)

### [Why this implementation?](https://www.youtube.com/watch?v=6oUW9IYbhEc)

### [Further use](https://www.youtube.com/watch?v=yR7k19YBqiw)





