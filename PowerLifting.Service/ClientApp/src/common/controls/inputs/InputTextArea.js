
export function InputTextArea({ label, onChange, propName, initialValue, rows = 3, cols = 85 }) {

  return (
    <>
      <p>{label}</p>
      <textarea onChange={(e) => setValue(e, propName, onChange)} value={initialValue ?? ''} rows={rows} cols={cols} />
    </>
  )
}

function setValue(event, propName, onChange) {
  var val = event.target.value;
  onChange(propName, val); //вызываем переданный хендлер, в который уходит: 1) имя свойства в стейте, 2)значение, которое в него надо положить.
}