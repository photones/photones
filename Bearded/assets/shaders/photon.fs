#version 150

in Fragment
{
    vec4 photon_position;
    float photon_radius;
    vec4 frag_position;
    vec4 color;
} f;

out vec4 color;

void main()
{
    // The color intensity of the photon fragment corresponds linearly with the
    // distance to the center of the photon
    float dx = abs(f.frag_position.x - f.photon_position.x);
    float dy = abs(f.frag_position.y - f.photon_position.y);
    float d = dx * dx + dy * dy;
    float r = f.photon_radius * f.photon_radius;
    float color_intensity = 1 - sqrt(d / r);
    color = color_intensity * f.color;
}
