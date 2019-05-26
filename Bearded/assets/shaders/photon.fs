#version 150

in Fragment
{
    vec4 color;
} f;

out vec4 color;

void main()
{
    color = f.color;
}
