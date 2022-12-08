# SV-Projekt
The github repository for our team's project for the Collective Behaviour class 2022/23

Our project is based on a paper by Ivanov & Palamas. 2022. Collective Adaptation in Multi-Agent Systems: How Predator Confusion Shapes Swarm-Like Behaviors, https://arxiv.org/abs/2209.06338  
We intend to reimplement the simulation described in the above paper.   
By the first report, we intend to study the paper and underlying concepts and possibly start planning our implementation.  
By the second report, we intend to have a working prototype of the solution and plan potential expansions of the work.  
By the third (final) report, we intend to polish our implementation and compare it to the original paper.  


## Running the project  
We are using UnityEngine version 2022.1.22f1 with the library ml-agents version 0.28.0 and pytorch version 1.7.1.
After cloning this repository and opening the folder "Collective behaviour project" with Unity, you can press play and it should work.  
For development purposes you should select the prey asset and change the behaviour type to "Heuristic" (manually testing inputs) or "Default" (for learning).
If you want to make a model and have it learn, you need to run the ml-agents program in a terminal and then press play in unity.  
this is an approximate command you can use `mlagents-learn results\prey.yaml --run-id=Prey`.
You should check out this tutorial to see what it is about https://www.youtube.com/watch?v=zPFU30tbyKs&list=PLzDRvYVwl53vehwiN_odYJkPBzcqFw110&index=1.
Or the mlagents github https://github.com/Unity-Technologies/ml-agents for written and up to date tutorials and installation instructions.