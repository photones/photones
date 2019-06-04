﻿#version 150

layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

in Vertex {
    vec4 position;
    vec4 color;
} v[];

out Fragment {
    vec4 color;
} f;


void rect(vec4 center, float size, vec4 color) {
    mat2 rot = mat2(0, -1, 1, 0); // counter clockwise
    vec2 x = vec2(size);
    // bottomleft, bottomright, topleft, topright
    vec2[4] corners = vec2[](x, rot*x, rot*rot*rot*x, rot*rot*x);

    for (int i=0; i<4; i++) {
        gl_Position = center + vec4(corners[i], 0.0, 0.0);
        f.color = color;
        EmitVertex();
    }

    EndPrimitive();
}


void main() {
    rect(v[0].position, 0.005, v[0].color);
}