#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aColor;

out vec3 fragColor;

uniform mat4 uView;
uniform mat4 uProjection;

void main(){
    fragColor = aColor;
    gl_Position = uProjection * uView * vec4(aPos, 1);
}
