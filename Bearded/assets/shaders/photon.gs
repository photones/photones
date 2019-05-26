#version 150

layout (points) in;
layout (triangle_strip, max_vertices = 20) out;

in Vertex {
    vec4 position;
    vec4 color;
} v[];

out Fragment {
    vec4 color;
} f;


void rect(vec4 position, float size, vec4 color, int i) {
    gl_Position = position + vec4(-size, -size, 0.0, 0.0);    // 1:bottom-left
    f.color = color;
    EmitVertex();
    gl_Position = position + vec4( size, -size, 0.0, 0.0);    // 2:bottom-right
    f.color = color;
    EmitVertex();
    gl_Position = position + vec4(-size,  size, 0.0, 0.0);    // 3:top-left
    f.color = color;
    EmitVertex();
    gl_Position = position + vec4( size,  size, 0.0, 0.0);    // 4:top-right
    f.color = color;
    EmitVertex();

    EndPrimitive();
}


void main() {
    rect(v[0].position, 0.003, v[0].color, 0);
    rect(v[0].position + vec4(-0.01, -0.01, 0.0, 0.0), 0.002, vec4(1.0, 0.0, 0.0, 1.0), 2);
    rect(v[0].position + vec4(0.01, -0.01, 0.0, 0.0), 0.002, vec4(1.0, 0.0, 0.0, 1.0), 2);
    rect(v[0].position + vec4(-0.01, 0.01, 0.0, 0.0), 0.002, vec4(1.0, 0.0, 0.0, 1.0), 2);
    rect(v[0].position + vec4(0.01, 0.01, 0.0, 0.0), 0.002, vec4(1.0, 0.0, 0.0, 1.0), 2);
}
