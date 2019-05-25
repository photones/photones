#version 150

layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

void build_rect(vec4 position)
{
    gl_Position = position + vec4(-0.002, -0.002, 0.0, 0.0);    // 1:bottom-left
    EmitVertex();
    gl_Position = position + vec4( 0.002, -0.002, 0.0, 0.0);    // 2:bottom-right
    EmitVertex();
    gl_Position = position + vec4(-0.002,  0.002, 0.0, 0.0);    // 3:top-left
    EmitVertex();
    gl_Position = position + vec4( 0.002,  0.002, 0.0, 0.0);    // 4:top-right
    EmitVertex();
    EndPrimitive();
}

void main() {
    build_rect(gl_in[0].gl_Position);
}
