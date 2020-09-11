﻿using UnityEngine;

public class EffectShower : MonoBehaviour
{
    private class FilledImage
    {
        public object UniqueObject { get; set; }
        public ImageWithNumber ImageWithNumber { get; }
        public float Value { get; set; }
        public float MaxValue { get; set; }
        public bool ReversedFill
        {
            get { return reversedFill; }
            set
            {
                reversedFill = value;
                ImageWithNumber.ClockFill = value;
            }
        }
        private bool reversedFill;

        public bool Active
        {
            get { return ImageWithNumber.gameObject.activeSelf; }
            set { ImageWithNumber.gameObject.SetActive(value); }
        }

        public FilledImage(ImageWithNumber imageWithNumber)
        {
            ImageWithNumber = imageWithNumber;
        }

        public void UpdateFill()
        => ImageWithNumber.FillAmount = (ReversedFill ? MaxValue - Value : Value) / MaxValue;
    }

    [SerializeField] private ImageWithNumber[] images;
    private FilledImage[] filledImages;

    private int length;

    private void Awake()
    {
        length = images.Length;
        filledImages = new FilledImage[length];

        for (int i = 0; i < length; i++)
        {
            filledImages[i] = new FilledImage(images[i]);
        }

        filledImages.ForEach(x => x.Active = false);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < length; i++)
        {
            if (filledImages[i].Active && filledImages[i].Value >= 0)
            {
                filledImages[i].Value -= Time.deltaTime;
                filledImages[i].UpdateFill();
            }

            if (filledImages[i].Value < 0 && !filledImages[i].ReversedFill)
            {
                filledImages[i].Active = false;
                filledImages[i].Value = 0;

                for (int j = i + 1; j < length; j++)
                {
                    var tmpPos = filledImages[j].ImageWithNumber.transform.position;
                    filledImages[j].ImageWithNumber.transform.position = filledImages[j - 1].ImageWithNumber.transform.position;
                    filledImages[j - 1].ImageWithNumber.transform.position = tmpPos;

                    var tmp = filledImages[j];
                    filledImages[j] = filledImages[j - 1];
                    filledImages[j - 1] = tmp;
                }

                i--;
            }
        }

        filledImages.ForEach(x => x.UpdateFill());
    }

    public void AddOrUpdate(Sprite effectIcon, EffectType effectType, float time, object uniqueObject = null)
    {
        foreach (var filledImage in filledImages)
        {
            if (filledImage.Active && filledImage.ImageWithNumber.Sprite == effectIcon && filledImage.UniqueObject == uniqueObject)
            {
                if (effectType == EffectType.Single)
                {
                    filledImage.Value = time;
                    filledImage.ImageWithNumber.Number++;
                    StartCoroutine(CoroutineT.Single(() => filledImage.ImageWithNumber.Number--, time));
                    break;
                }

                if (effectType == EffectType.Reloadable)
                {
                    filledImage.Value = time;
                    break;
                }
            }

            if (!filledImage.Active)
            {
                if (effectType == EffectType.Single)
                    filledImage.Value = time;
                else if (effectType == EffectType.Reloadable)
                    filledImage.Value = 0;

                filledImage.UniqueObject = uniqueObject;
                filledImage.MaxValue = time;
                filledImage.ReversedFill = EffectTypeToReverse(effectType);

                filledImage.ImageWithNumber.Sprite = effectIcon;
                filledImage.Active = true;

                if (effectType == EffectType.Single)
                {
                    filledImage.ImageWithNumber.NumberVisible = true;
                    filledImage.ImageWithNumber.Number = 1;

                    StartCoroutine(CoroutineT.Single(() => filledImage.ImageWithNumber.Number--, time));
                }
                else
                    filledImage.ImageWithNumber.NumberVisible = false;

                break;
            }
        }
    }

    public void TryRemove(Sprite effectIcon, object uniqueObject = null)
    {
        foreach (var filledImage in filledImages)
        {
            if (filledImage.Active && filledImage.ImageWithNumber.Sprite == effectIcon
            && filledImage.UniqueObject == uniqueObject)
            {
                if (filledImage.ImageWithNumber.Number > 0)
                {
                    filledImage.ImageWithNumber.Number--;

                    if (filledImage.ImageWithNumber.Number == 0)
                        filledImage.Value = 0;

                    break;
                }
            }
        }
    }

    public bool Contains(Sprite effectIcon, object uniqueObject = null)
    {
        foreach (var filledImage in filledImages)
        {
            if (filledImage.Active && filledImage.ImageWithNumber.Sprite == effectIcon
            && filledImage.UniqueObject == uniqueObject)
            {
                if (filledImage.ImageWithNumber.Number > 0)
                    return true;
            }
        }

        return false;
    }

    private bool EffectTypeToReverse(EffectType type)
    {
        switch (type)
        {
            case EffectType.Reloadable:
                return true;
            case EffectType.Single:
                return false;
            default:
                return false;
        }
    }
}