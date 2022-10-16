
export function InputTextArea({ label, onChange, propName, initialValue, rows = 3, cols = 85 }) {
  label = '  ' + label;
  initialValue = initialValue || '';

  return (
    <>
      <textarea value={initialValue} rows={rows} cols={cols} placeholder={label}
        onChange={(e) => setValue(e, propName, onChange)} />
    </>
  )
}

function setValue(event, propName, onChange) {
  var val = event.target.value;
  onChange(propName, val); //вызываем переданный хендлер, в который уходит: 1) имя свойства в стейте, 2)значение, которое в него надо положить.
}