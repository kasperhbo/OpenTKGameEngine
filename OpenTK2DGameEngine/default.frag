#version 330 core

in vec4 fColor;
in vec2 texCoords;
in float texId;

uniform sampler2D uTextures[8];

out vec4 color;

void main()
{
    if (texId > 0)
    {
        int id = int(texId);
        color = fColor * texture(uTextures[id], texCoords);
    } else {
        color = fColor;
    }
}