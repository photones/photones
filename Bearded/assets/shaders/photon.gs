#version 150

layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

in Vertex {
    vec4 position;
    vec4 color;
} v[];

out Fragment {
    vec4 color;
} f;


void main() {
    float size = 0.003;

    gl_Position = v[0].position + vec4(-size, -size, 0.0, 0.0);    // 1:bottom-left
    f.color = v[0].color;
    EmitVertex();
    gl_Position = v[0].position + vec4( size, -size, 0.0, 0.0);    // 2:bottom-right
    f.color = v[0].color;
    EmitVertex();
    gl_Position = v[0].position + vec4(-size,  size, 0.0, 0.0);    // 3:top-left
    f.color = v[0].color;
    EmitVertex();
    gl_Position = v[0].position + vec4( size,  size, 0.0, 0.0);    // 4:top-right
    f.color = v[0].color;
    EmitVertex();

    EndPrimitive();
}
