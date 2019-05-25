#version 150

// layout
layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

// uniforms
uniform mat4 modelviewMatrix;
uniform mat4 projectionMatrix;

// in & out
in Vertex
{
    vec3 position;
    vec4 color;
} vertex_in[];

out Fragment
{
    vec3 position;
    vec4 color;
} vertex_out;


void main()
{
    vec3 uX = vec3(0.5, 0.5, 0.5);
    vec3 uY = vec3(0.5, 0.5, 0.5);

    vec3 top = vertex_in[0].position - uY;
    vec3 bottom = vertex_in[0].position + uY;


    // topleft
    vec3 p = top - uX;
    gl_Position = projectionMatrix * modelviewMatrix * vec4(p, 1.0);
    vertex_out.position = p;
    vertex_out.color = vertex_in[0].color;
    EmitVertex();

    // topright
    p = top + uX;
    gl_Position = projectionMatrix * modelviewMatrix * vec4(p, 1.0);
    vertex_out.position = p;
    vertex_out.color = vertex_in[0].color;
    EmitVertex();

    // bottomleft
    p = bottom - uX;
    gl_Position = projectionMatrix * modelviewMatrix * vec4(p, 1.0);
    vertex_out.position = p;
    vertex_out.color = vertex_in[0].color;
    EmitVertex();

    // bottomright
    p = bottom + uX;
    gl_Position = projectionMatrix * modelviewMatrix * vec4(p, 1.0);
    vertex_out.position = p;
    vertex_out.color = vertex_in[0].color;
    EmitVertex();

    EndPrimitive();

}
