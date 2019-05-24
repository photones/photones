#version 150

in vec3 v_position;
in vec4 v_color;

out Vertex
{
    vec3 position;
    vec4 color;
} vertex_out;

void main()
{
    vertex_out.position = v_position;
    vertex_out.color = v_color;
}
