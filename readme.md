# 2D Olfactory Map
By Noé GUILLAUMIN

## General Info
- Unity version : 2022.3.5f1
- Render pipeline version: HDRP 14.0.11
- GitHub : https://github.com/Antoinoe/2D-Olfactory-Map
- Contact : noeguillaumin@gmail.com

## Project Description
Collider boxes are generally used to diffuse smells in virtual environments. However, this technique can become challenging when working in large outdoor environments. The more types of smells there are to include, the more difficult it is to maintain. 

Using textures, these olfactory zones can be defined more precisely and become more intuitive visually. Each texture corresponds to a smell, and the alpha value corresponds to the smell's intensity. Every few seconds, a C# script gets all of the textures and maps the player's position to the texture coordinates. It takes the alpha values from each pixel and the associated smell. These data can be transferred to a custom device that displays the appropriate smell to the user.

Please note that this project only focuses on the texture reading part, not the communication between Unity and any external olfactory device. 

This project does have some limitations.
Firstly, it only works in 2D (x,z). Secondly, having too many textures on a large terrain can significantly reduce performance. 

For future works, I'm interested in dynamically modifying the texture through a shader so it can interact with the virtual environment, such as the wind.

