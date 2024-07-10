#version 400
uniform mat4 modelview; 
uniform mat4 projection;
layout(location = 0) in vec4 vp;
out vec4 texCoord;

void main()
{
  vec4 position = projection * modelview * vec4(vp);
  gl_Position = position.xyww;
  texCoord = vp;
}