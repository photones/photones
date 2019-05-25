#version 150

in Fragment
{
    vec3 position;
    vec4 color;
} vertex_in;

out vec4 fragColor;

void main()
{
    //fragColor = vertex_in.color;
    fragColor = vec4(1.0, 0.5, 0.5, 0.5);
}
