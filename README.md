# Level Generator for Mario Game using Genetic Algorithm

Example of application of genetic algorithm for Super Mario Bross Level Generator. Project developed in the course of Artificial Intelligence applied to the Digital Games - Fatec Americana.

# Super Mario Bross (Game)

In Super Mario Bros., the player takes on the role of Mario, the protagonist of the series. Mario's younger brother, Luigi, is controlled by the second player in the game's multiplayer mode and assumes the same plot role and functionality as Mario. The objective is to race through the Mushroom Kingdom, survive the main antagonist Bowser's forces, and save Princess Toadstool. The game is a side-scrolling platformer; the player moves from the left side of the screen to the right side in order to reach the flag pole at the end of each level.

<p align="center">
  <img src="https://super.abril.com.br/wp-content/uploads/2018/03/mario.png" width="600"/>
</p>

## Genetic Algorithm (GA)

Text ...

### Implementation

1.  Generate the initial population of individuals randomly (First generation).
2.  Evaluate the fitness of each individual in that population (time limit, sufficient fitness achieved, etc)
3.  Repeat the following regenerational steps until termination:
4.  Select the best-fit individuals for reproduction (Parents)
5.  Breed new individuals through crossover and mutation operations to give birth to child.
6.  Evaluate the individual fitness of new individuals.
7.  Replace least-fit population with new individuals.

### Individual (Chromosome)

Text ...

### Fitness Function

Text ...

### Elitism 

A practical variant of the general process of constructing a new population is to allow the best organism(s) from the current generation to carry over to the next, unaltered. This strategy is known as elitist selection and guarantees that the solution quality obtained by the GA will not decrease from one generation to the next.

### Tournament Selection

Tournament Selection is a Selection Strategy used for selecting the fittest candidates from the current generation in a Genetic Algorithm. These selected candidates are then passed on to the next generation. In a K-way tournament selection, we select k-individuals and run a tournament among them. Only the fittest candidate amongst those selected candidates is chosen and is passed on to the next generation. In this way many such tournaments take place and we have our final selection of candidates who move on to the next generation. It also has a parameter called the selection pressure which is a probabilistic measure of a candidate’s likelihood of participation in a tournament. If the tournament size is larger, weak candidates have a smaller chance of getting selected as it has to compete with a stronger candidate. The selection pressure parameter determines the rate of convergence of the GA. More the selection pressure more will be the Convergence rate. GAs are able to identify optimal or near-optimal solutions over a wide range of selection pressures. Tournament Selection also works for negative fitness values.

1.  Select k individuals from the population and perform a tournament amongst them
2.  Select the best individual from the k individuals
3.  Repeat process 1 and 2 until you have the desired amount of population

### One-Point Crossover

Text ...

### Random Resetting Mutation

Text ...

## Experiments and Results

Initial setup of the experiment.



## References

* Dahlskog, S., Togelius, J.: [Patterns as Objectives for Level Generation](https://muep.mau.se/handle/2043/15713).In: Proceedings of the Second Workshop on Design Patterns in Games, 2013.

## Licença

    Copyright 2020 Kleber de Oliveira Andrade
    
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
