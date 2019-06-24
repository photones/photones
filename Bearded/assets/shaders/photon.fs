#version 150

in Fragment
{
    vec4 color;
} f;

out vec4 color;

void main()
{
    // gl_FragCoord is (x,y)
    color = (f.color + vec4(gl_FragCoord.x / 1000, gl_FragCoord.y / 1000, 0.0, 1.0)) / 2;
}
