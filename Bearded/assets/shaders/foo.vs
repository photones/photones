#version 150

uniform mat4 projection;
uniform mat4 view;

in vec3 v_position;
in vec4 v_color;

void main()
{
    gl_Position = projection * view * vec4(v_position, 1.0);
}
