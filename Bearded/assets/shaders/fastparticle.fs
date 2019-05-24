#version 150

in Fragment
{
    vec3 position;
    vec4 color;
} vertex_in;

out vec4 fragColor;

void main()
{
    fragColor = vertex_in.color;
}
