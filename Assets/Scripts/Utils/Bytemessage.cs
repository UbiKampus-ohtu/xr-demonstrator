using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bytemessage {
  private int finalSize;
  private Queue<byte[]> byteBlocks;  

  public Bytemessage() {
    finalSize = 0;
    byteBlocks = new Queue<byte[]>{};
  }

  public Bytemessage transform(Vector3 position, Quaternion rotation) {
    float [] transformComponents = {
      position.x,
      position.y,
      position.z,
      rotation.x,
      rotation.y,
      rotation.z,
      rotation.w
    };

    int bytesToCopy = transformComponents.Length * 4;
    byte [] result = new byte[bytesToCopy];
    Buffer.BlockCopy(transformComponents, 0, result, 0, bytesToCopy);

    byteBlocks.Enqueue(result);

    return this;
  }

  public byte [] get() {
    int cursor = 0;
    byte [] buffer = new byte[finalSize];

    while (byteBlocks.Count > 0) {
      byte [] byteBlock = byteBlocks.Dequeue();
      Buffer.BlockCopy(byteBlock, 0, buffer, cursor, cursor + byteBlock.Length);
      cursor += byteBlock.Length;
    }

    return buffer;
  }
}
