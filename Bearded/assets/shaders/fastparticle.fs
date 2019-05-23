#version 150

in vec4 p_color;

out vec4 fragColor;

void main()
{
	vec4 c = p_color;
    fragColor = c;
}
