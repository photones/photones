﻿#version 150

uniform mat4 projection;
uniform mat4 view;

in vec3 v_position;
in vec4 v_color;

out Vertex
{
    vec4 position;
    vec4 color;
} out_vertex;

void main()
{
    out_vertex.position = projection * view * vec4(v_position, 1.0);
    out_vertex.color = v_color;
}
